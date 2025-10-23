using J2N;
using RegexRenamer.Forms;
using Kavita;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace RegexRenamer.Rename
{
    public enum MetadataType
    {
        Title,
        Series,
        Writer,
        Volume,
        Language,
    }
    public static class MetadataPreviewExtensions
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

        private static ReadingItemService parser = null;

        public static ComicInfo GetMetadata(this FileInfo pThis)
        {
            if (parser == null)
                parser = new ReadingItemService(new DirectoryService());

            var ci = parser.GetComicInfo(pThis.FullName, true);

            // force release file handles using pinvoke
            return ci;
        }

        public static string GetMetadataValue(this ComicInfo pThis, MetadataType pType)
        {
            return pType switch
            {
                MetadataType.Title => pThis.Title ?? string.Empty,
                MetadataType.Series => pThis.Series ?? string.Empty,
                MetadataType.Writer => pThis.Writer ?? string.Empty,
                MetadataType.Volume => pThis.Volume ?? string.Empty,
                MetadataType.Language => pThis.LanguageISO ?? string.Empty,
                _ => throw new NotImplementedException(),
            };
        }

        public static void SetMetadataValue(this ComicInfo pThis, MetadataType pType, string pValue)
        {
            switch (pType)
            {
                case MetadataType.Title:
                    pThis.Title = pValue;
                    break;
                case MetadataType.Series:
                    pThis.Series = pValue;
                    break;
                case MetadataType.Writer:
                    pThis.Writer = pValue;
                    break;
                case MetadataType.Volume:
                    pThis.Volume = pValue;
                    break;
                case MetadataType.Language:
                    pThis.LanguageISO = pValue;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static int BuildMetadataPreview(this List<Tuple<RenameItemInfo, ComicInfo, ComicInfo>> pThis,MetadataType colType, string matchingPattern, string replacePattern,  
            AutoNumberingInfo numInfo, ChangeCaseOption changeCaseInfo, RegexModifierInfo modifierInfo)
        {
            int retVal = 0;

            const string rxDoller = @"(?<=(?:^|[^$])(?:\$\$)*)\$";  // regex for an actual (non-escaped) doller sign
            var helper = new MatchEvaluatorHelper(changeCaseInfo);
            var matchEval = new MatchEvaluator(helper.MatchEvalChangeCase);

            // generate preview
            if (!string.IsNullOrWhiteSpace(matchingPattern) && !string.IsNullOrWhiteSpace(replacePattern))
            {
                // get regex options according to user selections
                var (options, count) = modifierInfo.GetRegexOptions();
                //main regex
                Regex regex = new Regex(matchingPattern, options);

                // auto numbering
                int numCurrent = 0, numInc = 0, numStart = 0, numReset = 0;
                string numFormatted = "";
                bool doingAutoNum = false;
                bool doingAutoNumLetter = false;  // number sequence is actually a-z letter sequence
                bool doingAutoNumLetterUpper = false;  // letter sequence is uppercase

                if (numInfo.ValidNumber && Regex.IsMatch(replacePattern, rxDoller + "#"))
                    doingAutoNum = true;

                Match match = Regex.Match(numInfo.NumberingStart, @"^(([a-z]+)|([A-Z]+))$");
                doingAutoNumLetter = match.Success;
                doingAutoNumLetterUpper = match.Success && match.Groups[3].Length > 0;

                if (doingAutoNum)
                {
                    if (doingAutoNumLetter)
                    {
                        numStart = SequenceLetterToNumber(numInfo.NumberingStart.ToLower());
                    }
                    else
                    {
                        numStart = Int32.Parse(numInfo.NumberingStart);
                    }

                    numInc = Int32.Parse(numInfo.NumberingIncStep);
                    numReset = Int32.Parse(numInfo.NumberingReset);
                }
                numCurrent = numStart - numInc;  // back up one

                // regex each filename
                string userReplacePattern = replacePattern;
                if (doingAutoNum)
                {
                    userReplacePattern = Regex.Replace(userReplacePattern, rxDoller + @"(\d+)" + rxDoller + "#", "$${$1}$$#");
                }

                for (int idx = 0; idx < pThis.Count; idx++)
                {
                    string replacePatternFinal;

                    if (doingAutoNum)
                    {
                        numCurrent += numInc;

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

                        replacePatternFinal = Regex.Replace(userReplacePattern, rxDoller + "#", numFormatted);
                    }
                    else
                    {
                        replacePatternFinal = userReplacePattern;
                    }


                    // enclose the replace pattern with \n so we can do Title-case 
                    if (changeCaseInfo != ChangeCaseOption.NoChange)
                        replacePatternFinal = "\n" + replacePatternFinal + "\n";  // delimit change-case boundaries

                    // do replace and store preview
                    var newValPreview = regex.Replace(pThis[idx].Item1.Name, replacePatternFinal, count);

                    // if change case selected, then do it now, looks for all \n delimited sections,
                    if (changeCaseInfo != ChangeCaseOption.NoChange)
                        newValPreview = Regex.Replace(newValPreview, @"\n([^\n]*)\n", matchEval);

                    if(!string.IsNullOrWhiteSpace(newValPreview))
                        pThis[idx].Item2.SetMetadataValue(colType, newValPreview.Trim());
                    retVal = string.IsNullOrWhiteSpace(newValPreview) ? 0 : 1;
                }
            }
            else
            {
                retVal = 1;
                // clear preview
                for (int afi = 0; afi < pThis.Count; afi++)
                {
                    pThis[afi].Item2.SetMetadataValue(colType, "");
                }
            }

            return retVal;
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
    }
}
