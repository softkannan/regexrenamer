using iText.StyledXmlParser.Jsoup.Parser;
using RegexRenamer.Kavita;
using RegexRenamer.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRenamer.RegexProcessor
{
    public class AutoNumberingInfo
    {
        public string NumberingStart;
        public string NumberingIncStep;
        public string NumberingReset;
        public string NumberingPad;
        public bool ValidNumber;
    }

    public class KavitaInfo
    {
        public bool ShowKavitaPreview;
        public string KavitaRoot;
        public LibraryType KavitaLibType;
    }

    public enum ChangeCaseOption
    {
        NoChange,
        Uppercase,
        Lowercase,
        Titlecase,
        CleanName
    }

    public class RegexOptionsInfo
    {
        public bool ModifierG;
        public bool ModifierI;
        public bool ModifierX;
    }


    public class RegexProcessor
    {
        private readonly Kavita.ReadingItemService _parser = new Kavita.ReadingItemService(new Kavita.DirectoryService());

        private List<RRItem> _listOfFiles;

        private string _matchingPattern;
        private string _userReplacePattern;

        private AutoNumberingInfo _numInfo;
        private KavitaInfo _kavitaInfo;
        private ChangeCaseOption _changeCaseInfo;
        private RegexOptionsInfo _modifierInfo;

        public RegexProcessor(List<RRItem> listOfFiles, string matchingPattern, string userReplacePattern,
        AutoNumberingInfo numInfo, ChangeCaseOption changeCaseInfo, KavitaInfo kavitaInfo, RegexOptionsInfo modifierInfo ) { 

            _listOfFiles = listOfFiles;
            _matchingPattern = matchingPattern;
            _userReplacePattern = userReplacePattern;
            _numInfo = numInfo;
            _changeCaseInfo = changeCaseInfo;
            _kavitaInfo = kavitaInfo;
            _modifierInfo = modifierInfo;
        }

        private Tuple<RegexOptions, int> GetRegexOptions()
        {
            RegexOptions options;
            int count;
            // compile regex
            options = RegexOptions.None | RegexOptions.Compiled;
            count = _modifierInfo.ModifierG ? -1 : 1;
            if (_modifierInfo.ModifierI) { options |= RegexOptions.IgnoreCase; }
            if (_modifierInfo.ModifierX) { options |= RegexOptions.IgnorePatternWhitespace; }

            return Tuple.Create(options, count);
        }

        private string MatchEvalChangeCase(Match match)
        {
            TextInfo ti = new CultureInfo("ta").TextInfo;

            switch(_changeCaseInfo)
            {
                case ChangeCaseOption.Uppercase:
                    return ti.ToUpper(match.Groups[1].Value);
                case ChangeCaseOption.Lowercase:
                    return ti.ToLower(match.Groups[1].Value);
                case ChangeCaseOption.Titlecase:
                    return ti.ToTitleCase(match.Groups[1].Value.ToLower());
                case ChangeCaseOption.CleanName:
                    return match.Groups[1].Value.ToCleanFileName();
                case ChangeCaseOption.NoChange:
                default:
                    return match.Groups[1].Value;
            }
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

        private void UpdateKavitaCheck(RRItem match, string kavithaRoot, Kavita.LibraryType libType)
        {
            var parseInfo = _parser.ParseFile(match.PreviewFullPath, kavithaRoot, kavithaRoot, libType, true);
            match.ParseInfo = parseInfo;

            //var comicInfo = parser.GetComicInfo(match.PreviewFullPath, true); //, kavithaRoot, libType);
            //match.ComicInfo = comicInfo;

        }

        public void BuildPreviewData()
        {
            const string rxDoller = @"(?<=(?:^|[^$])(?:\$\$)*)\$";  // regex for an actual (non-escaped) doller sign

            // generate preview
            if (!string.IsNullOrWhiteSpace(_matchingPattern))
            {
                var (options, count) = GetRegexOptions();

                //main regex
                Regex regex = new Regex(_matchingPattern, options);

                // auto numbering
                int numCurrent = 0, numIncStep = 0, numStart = 0, numReset = 0;
                string numFormatted = "";
                bool doingAutoNum = false;
                bool doingAutoNumLetter = false;  // number sequence is actually a-z letter sequence
                bool doingAutoNumLetterUpper = false;  // letter sequence is uppercase

                if (_numInfo.ValidNumber && Regex.IsMatch(_userReplacePattern, rxDoller + "#"))
                    doingAutoNum = true;

                Match match = Regex.Match(_numInfo.NumberingStart, @"^(([a-z]+)|([A-Z]+))$");
                doingAutoNumLetter = match.Success;
                doingAutoNumLetterUpper = match.Success && match.Groups[3].Length > 0;

                // validate numbering inputs and parse to int
                if (doingAutoNum)
                {
                    // if starting is letter sequence, then increment step and reset must be 1
                    if (doingAutoNumLetter)
                        numStart = SequenceLetterToNumber(_numInfo.NumberingStart.ToLower());
                    else
                        numStart = Int32.Parse(_numInfo.NumberingStart);

                    numIncStep = Int32.Parse(_numInfo.NumberingIncStep);
                    // Reset to starting number every x files
                    numReset = Int32.Parse(_numInfo.NumberingReset);
                }
                // backup one step so first file is correct after initial increment
                numCurrent = numStart - numIncStep;  // back up one

                // 
                if (doingAutoNum)
                {
                    _userReplacePattern = Regex.Replace(_userReplacePattern, rxDoller + @"(\d+)" + rxDoller + "#", "$${$1}$$#");
                }

                for (int afi = 0; afi < _listOfFiles.Count; afi++)
                {
                    // check if matches
                    _listOfFiles[afi].ComicInfo = null;
                    _listOfFiles[afi].ParseInfo = null;
                    _listOfFiles[afi].Matched = regex.IsMatch(_listOfFiles[afi].Name);

                    // if not, bail early, don't incrememnt autonum
                    if (!_listOfFiles[afi].Matched)
                    {
                        _listOfFiles[afi].Preview = _listOfFiles[afi].Name;
                        continue;
                    }

                    // increment autonum and replace numbering pattern with current number
                    string replacePattern;
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
                                    numFormatted = numCurrent.ToString(_numInfo.NumberingPad);
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

                        replacePattern = Regex.Replace(_userReplacePattern, rxDoller + "#", numFormatted);
                    }
                    else
                    {
                        replacePattern = _userReplacePattern;
                    }

                    // enclose the replace pattern with \n so we can do Title-case 
                    if (_changeCaseInfo != ChangeCaseOption.NoChange)
                        replacePattern = "\n" + replacePattern + "\n";  // delimit change-case boundaries

                    // do replace and store preview
                    _listOfFiles[afi].Preview = regex.Replace(_listOfFiles[afi].Name, replacePattern, count);

                    // if change case selected, then do it now, looks for all \n delimited sections,
                    if (_changeCaseInfo != ChangeCaseOption.NoChange)
                        _listOfFiles[afi].Preview = Regex.Replace(_listOfFiles[afi].Preview, @"\n([^\n]*)\n", new MatchEvaluator(MatchEvalChangeCase));

                    // if kavita preview is selected, then get the kavita parse info and attach to the item
                    if (_kavitaInfo.ShowKavitaPreview)
                    {
                       _kavitaInfo.KavitaRoot ??= Directory.GetDirectoryRoot(_listOfFiles[afi].Fullpath);
                       UpdateKavitaCheck(_listOfFiles[afi], _kavitaInfo.KavitaRoot, _kavitaInfo.KavitaLibType);
                    }

                    // if preview is empty, use original name
                    if (_listOfFiles[afi].Preview.Length == 0)
                        _listOfFiles[afi].Preview = _listOfFiles[afi].Name;
                }

            }
            else  // cmbMatch.Text == ""
            {
                foreach (RRItem file in _listOfFiles)
                {
                    file.Preview = file.Name;
                    file.Matched = false;
                }
            }
        }
    }
}
