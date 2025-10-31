using Kavita;
using Kavita.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRenamer.Rename;

public class AutoNumberingInfo
{
    public string NumberingStart { get; init; }
    public string NumberingIncStep { get; init; }
    public string NumberingReset { get; init; }
    public string NumberingPad { get; init; }
    public bool ValidNumber { get; init; }
}

public class KavitaInfo
{
    public bool ShowPreview { get; init; }
    public string KavitaRoot { get; init; }
    public LibraryType KavitaLibType { get; init; }

    public bool UseMetadata { get; init; }
}

public enum ChangeCaseOption
{
    NoChange,
    Uppercase,
    Lowercase,
    Titlecase,
    CleanName
}

public class RegexModifierInfo
{
    public bool ReplaceEveryMatch { get; init; }
    public bool IgnoreCase { get; init; }
    public bool IgnorePatternWhitespace { get; init; }
}



public static class RenamePreviewExtensions
{
    private class MatchEvaluatorHelper
    {
        private ChangeCaseOption _changeCaseInfo;
        private TextInfo _ti = new CultureInfo("us").TextInfo;

        public MatchEvaluatorHelper(ChangeCaseOption changeCaseInfo)
        {
            _changeCaseInfo = changeCaseInfo;
        }

        public string MatchEvalChangeCase(Match match)
        {
            switch (_changeCaseInfo)
            {
                case ChangeCaseOption.Uppercase:
                    return _ti.ToUpper(match.Groups[1].Value);
                case ChangeCaseOption.Lowercase:
                    return _ti.ToLower(match.Groups[1].Value);
                case ChangeCaseOption.Titlecase:
                    return _ti.ToTitleCase(match.Groups[1].Value.ToLower());
                case ChangeCaseOption.CleanName:
                    return match.Groups[1].Value.ToCleanFileName();
                case ChangeCaseOption.NoChange:
                default:
                    return match.Groups[1].Value;
            }
        }
    }

    public static Regex CreateGlobFilter(this string filterText, bool globFilter)
    {
        Regex filter = null;
        if (!string.IsNullOrWhiteSpace(filterText))
        {
            if (globFilter && filterText == "*.*")  // convert to "*" (include files with no extension)
                filterText = "*";

            if (globFilter)  // convert glob to regex
                filterText = "^" + Regex.Escape(filterText).Replace("\\*", ".*").Replace("\\?", ".") + "$";

            filter = new Regex(filterText, RegexOptions.IgnoreCase);
        }
        return filter;
    }

    private static ReadingItemService parser = null;

    public static void BuildPreview(this FilesStore pThis, string matchingPattern, string replacePattern,
        AutoNumberingInfo numInfo, ChangeCaseOption changeCaseInfo, KavitaInfo kavitaInfo, RegexModifierInfo modifierInfo)
    {
        if (parser == null)
            parser = new ReadingItemService(new DirectoryService());

        const string rxDoller = @"(?<=(?:^|[^$])(?:\$\$)*)\$";  // regex for an actual (non-escaped) doller sign
        
        var helper = new MatchEvaluatorHelper(changeCaseInfo);
        var matchEval = new MatchEvaluator(helper.MatchEvalChangeCase);
        IReadOnlyList<RenameItemInfo>  files = pThis.Files;
        var userReplacePatternTemp = replacePattern;
        var showKavitaPreview = kavitaInfo?.ShowPreview ?? false;

        // generate preview
        if (!string.IsNullOrWhiteSpace(matchingPattern))
        {
            // get regex options according to user selections
            var (options, count) = modifierInfo.GetRegexOptions();
            //main regex
            Regex regex = new Regex(matchingPattern, options);

            // auto numbering
            int numCurrent = 0, numIncStep = 0, numStart = 0, numReset = 0;
            string numFormatted = "";
            bool doingAutoNum = false;
            bool doingAutoNumLetter = false;  // number sequence is actually a-z letter sequence
            bool doingAutoNumLetterUpper = false;  // letter sequence is uppercase

            if (numInfo.ValidNumber && Regex.IsMatch(replacePattern, rxDoller + "#"))
                doingAutoNum = true;

            Match match = Regex.Match(numInfo.NumberingStart, @"^(([a-z]+)|([A-Z]+))$");
            doingAutoNumLetter = match.Success;
            doingAutoNumLetterUpper = match.Success && match.Groups[3].Length > 0;

            // validate numbering inputs and parse to int
            if (doingAutoNum)
            {
                // if starting is letter sequence, then increment step and reset must be 1
                if (doingAutoNumLetter)
                    numStart = SequenceLetterToNumber(numInfo.NumberingStart.ToLower());
                else
                    numStart = Int32.Parse(numInfo.NumberingStart);

                numIncStep = Int32.Parse(numInfo.NumberingIncStep);
                // Reset to starting number every x files
                numReset = Int32.Parse(numInfo.NumberingReset);
            }
            // backup one step so first file is correct after initial increment
            numCurrent = numStart - numIncStep;  // back up one

            // 
            if (doingAutoNum)
            {
                userReplacePatternTemp = Regex.Replace(replacePattern, rxDoller + @"(\d+)" + rxDoller + "#", "$${$1}$$#");
            }

            for (int idx = 0; idx < files.Count; idx++)
            {
                // check if matches
                files[idx].ComicInfo = null;
                files[idx].ParseInfo = null;
                files[idx].Matched = regex.IsMatch(files[idx].Name);

                // if not, bail early, don't incrememnt autonum
                if (!files[idx].Matched)
                {
                    files[idx].Preview = files[idx].Name;
                    continue;
                }
                if (files[idx].Skip)
                    continue;

                // increment autonum and replace numbering pattern with current number
                string replacePatternFinal;
                if (doingAutoNum)
                {
                    // increment number with current step
                    numCurrent += numIncStep;

                    // Apply number reset logic
                    if (numReset != 0 && (numCurrent - numStart) % numReset == 0)
                        numCurrent = numStart;

                    if (numFormatted != "$#")  // basic int overflow & negative number detection
                    {
                        if (!doingAutoNumLetter)  // number sequence
                        {
                            if (numCurrent < 0)
                                numFormatted = "$#";
                            else
                                numFormatted = numCurrent.ToString(numInfo.NumberingPad);
                        }
                        else  // letter sequence
                        {
                            if (numCurrent < 1)
                                numFormatted = "$#";
                            else if (doingAutoNumLetterUpper)
                                numFormatted = SequenceNumberToLetter(numCurrent).ToUpper();
                            else
                                numFormatted = SequenceNumberToLetter(numCurrent);
                        }
                    }

                    replacePatternFinal = Regex.Replace(userReplacePatternTemp, rxDoller + "#", numFormatted);
                }
                else
                {
                    replacePatternFinal = userReplacePatternTemp;
                }

                // enclose the replace pattern with \n so we can do Title-case 
                if (changeCaseInfo != ChangeCaseOption.NoChange)
                    replacePatternFinal = "\n" + replacePatternFinal + "\n";  // delimit change-case boundaries

                // do replace and store preview
                files[idx].Preview = regex.Replace(files[idx].Name, replacePatternFinal, count);

                // if change case selected, then do it now, looks for all \n delimited sections,
                if (changeCaseInfo != ChangeCaseOption.NoChange)
                    files[idx].Preview = Regex.Replace(files[idx].Preview, @"\n([^\n]*)\n", matchEval);

                // if preview is empty, use original name
                if (files[idx].Preview.Length == 0)
                    files[idx].Preview = files[idx].Name;

                // if kavita preview is selected, then get the kavita parse info and attach to the item
                if (showKavitaPreview)
                {
                    kavitaInfo.BuildKavitaPreview(files[idx], parser);
                }
            }

        }
        else  // cmbMatch.Text == ""
        {
            for (int idx = 0; idx < files.Count; idx++)
            {
                files[idx].Matched = false;
                if (files[idx].Skip)
                {
                    files[idx].Skip = false;
                    continue;
                }
                files[idx].Preview = files[idx].Name;
                // if kavita preview is selected, then get the kavita parse info and attach to the item
                if (showKavitaPreview)
                {
                    kavitaInfo.BuildKavitaPreview(files[idx], parser);
                }
            }
        }
    }

    public static void BuildPreview(this FilesStore pThis, string matchingPattern, string replacePattern,
        AutoNumberingInfo numInfo, ChangeCaseOption changeCaseInfo, RegexModifierInfo modifierInfo)
    {
        pThis.BuildPreview(matchingPattern, replacePattern, numInfo, changeCaseInfo, null, modifierInfo);
    }

    private static Tuple<RegexOptions, int> GetRegexOptions(this RegexModifierInfo pThis)
    {
        RegexOptions options;
        int count;
        // compile regex
        options = RegexOptions.None | RegexOptions.Compiled;
        count = pThis.ReplaceEveryMatch ? -1 : 1;
        if (pThis.IgnoreCase) { options |= RegexOptions.IgnoreCase; }
        if (pThis.IgnorePatternWhitespace) { options |= RegexOptions.IgnorePatternWhitespace; }

        return Tuple.Create(options, count);
    }

    private static string SequenceNumberToLetter(int i)
    {
        int dividend = i;
        string columnName = String.Empty;

        while (dividend > 0)
        {
            int modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(97 + modulo) + columnName;  // note: A-Z = 65-90, a-z = 97-122
            dividend = (dividend - modulo) / 26;
        }

        return columnName;
    }
    private static int SequenceLetterToNumber(string letter)
    {
        int number = 0;
        int pow = 1;
        for (int i = letter.Length - 1; i >= 0; i--)
        {
            number += (letter[i] - 'a' + 1) * pow;
            pow *= 26;
        }

        return number;
    }

    private static void BuildKavitaPreview(this KavitaInfo pThis, RenameItemInfo match, ReadingItemService parser)
    {
        var kavitaRoot = pThis.KavitaRoot;
        kavitaRoot ??= Directory.GetDirectoryRoot(match.Fullpath);

        var parseInfo = parser.ParseFile(match.PreviewFullPath, kavitaRoot, kavitaRoot, pThis.KavitaLibType, pThis.UseMetadata);
        match.ParseInfo = parseInfo;

        //var comicInfo = parser.GetComicInfo(match.PreviewFullPath, true); //, kavithaRoot, libType);
        //match.ComicInfo = comicInfo;
    }
}
