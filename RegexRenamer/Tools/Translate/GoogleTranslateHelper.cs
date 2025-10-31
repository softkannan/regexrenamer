using EpubSharp;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RegexRenamer.Tools.Translate
{
    /// <summary>
    /// https://github.com/ssut/py-googletrans
    /// </summary>
    public static class GoogleTranslateHelper
    {
        /// <summary>
        /// https://translate.google.com
        /// https://translate.googleapis.com
        /// Free endpoint 1: https://clients5.google.com/translate_a/t?client=dict-chrome-ex&sl=en&tl=it&q=body
        /// Free endpoint 2: https://translate.googleapis.com/translate_a/single?client=gtx&dt=t&sl=en&tl=it&q=body
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <returns></returns>
        /// <exception cref="GoogleTranslateIPBannedException"></exception>
        public static async Task<TranslationResult> TranslateText(this string pThis,  string fromLanguage = "en", string toLanguage = "ta")
        {
            //var url = $"https://translate.google.com/?sl={fromLanguage}&tl={toLanguage}&text={HttpUtility.UrlEncode(pThis)}&op=translate";
            
            var client = "gtx"; // valid client values: gt, gtx

            var url = $"https://translate.googleapis.com/translate_a/single?client={client}" +
                $"&sl={fromLanguage}&tl={toLanguage}&hl=en" +
                $"&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t" +
                $"&ie=UTF-8&oe=UTF-8" +
                $"&otf=1&ssel=0&tsel=0&kc=7" +
                $"&q={HttpUtility.UrlEncode(pThis)}";

            var result = string.Empty;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.AcceptCharset.ParseAdd("UTF-8");
                try
                {
                    result  = await httpClient.GetStringAsync(url);
                }
                catch (HttpRequestException ex) when (ex.Message.Contains("503"))
                {
                    throw new GoogleTranslateIPBannedException(GoogleTranslateIPBannedException.Operation.Translation);
                }
            }
            return ResponseToTranslateResultParse(result, pThis, fromLanguage, toLanguage, false);
        }

        private static GoogleKeyTokenGenerator _generator = new GoogleKeyTokenGenerator();


        /// <summary>
        /// https://translate.google.com/?sl={fromLanguage}&tl={toLanguage}&text={HttpUtility.UrlEncode(pThis)}&op=translate
        /// https://translate.google.com 
        /// https://translate.googleapis.com
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <param name="additionInfo"></param>
        /// <returns></returns>
        /// <exception cref="GoogleTranslateIPBannedException"></exception>
        public static async Task<TranslationResult> TranslateWithTokenText(this string pThis,  string fromLanguage = "en", string toLanguage = "ta", bool additionInfo = true)
        {
            var client = "webapp";

            string token = await _generator.GenerateAsync(pThis);

            var url = $"https://translate.google.com/translate_a/single?client={client}" +
                $"&sl={fromLanguage}&tl={toLanguage}&hl=en" +
                $"&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t" +
                $"&ie=UTF-8&oe=UTF-8" +
                $"&otf=1&ssel=0&tsel=0&kc=7" +
                $"&tk={token}" +
                $"&q={HttpUtility.UrlEncode(pThis)}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.AcceptCharset.ParseAdd("UTF-8");
                var result = string.Empty;
                try
                {
                    result = await httpClient.GetStringAsync(url);
                }
                catch (HttpRequestException ex) when (ex.Message.Contains("503"))
                {
                    throw new GoogleTranslateIPBannedException(GoogleTranslateIPBannedException.Operation.Translation);
                }
                
                return ResponseToTranslateResultParse(result, pThis, fromLanguage, toLanguage, additionInfo);
            }
        }
        private static TranslationResult ResponseToTranslateResultParse(string result, string sourceText,
            string sourceLanguage, string targetLanguage, bool additionInfo)
        {
            TranslationResult translationResult = new TranslationResult();

            JToken tmp = JsonConvert.DeserializeObject<JToken>(result);

            string originalTextTranscription = null, translatedTextTranscription = null;

            var mainTranslationInfo = tmp[0];

            GetMainTranslationInfo(mainTranslationInfo, out var translation,
                ref originalTextTranscription, ref translatedTextTranscription);

            translationResult.FragmentedTranslation = translation;
            translationResult.OriginalText = sourceText;

            translationResult.OriginalTextTranscription = originalTextTranscription;
            translationResult.TranslatedTextTranscription = translatedTextTranscription;

            translationResult.Corrections = GetTranslationCorrections(tmp);

            translationResult.SourceLanguage = sourceLanguage;
            translationResult.TargetLanguage = targetLanguage;

            if (tmp[8] is JArray languageDetections)
                translationResult.LanguageDetections = GetLanguageDetections(languageDetections).ToArray();


            if (!additionInfo)
                return translationResult;

            translationResult.ExtraTranslations =
                TranslationInfoParse<ExtraTranslations>(tmp[1]);

            translationResult.Synonyms = tmp.Count() >= 12
                ? TranslationInfoParse<Synonyms>(tmp[11])
                : null;

            translationResult.Definitions = tmp.Count() >= 13
                ? TranslationInfoParse<Definitions>(tmp[12])
                : null;

            translationResult.SeeAlso = tmp.Count() >= 15
                ? GetSeeAlso(tmp[14])
                : null;

            return translationResult;
        }


        public static async Task<TranslationResult> TranslateAsync(string originalText, string fromLanguage, string toLanguage)
        {
            return await TranslateWithTokenText(originalText, fromLanguage, toLanguage, true);
        }

        public static async Task<TranslationResult> TranslateAsync(TranslateItem item)
        {
            return await TranslateAsync(item.OriginalText, item.FromLanguage, item.ToLanguage);
        }

        private static T TranslationInfoParse<T>(JToken response) where T : TranslationInfoParser
        {
            if (!response.HasValues)
                return null;

            T translationInfoObject = TranslationInfoParser.Create<T>();

            foreach (var item in response)
            {
                string partOfSpeech = (string)item[0];

                JToken itemToken = translationInfoObject.ItemDataIndex == -1 ? item : item[translationInfoObject.ItemDataIndex];

                //////////////////////////////////////////////////////////////
                // I delete the white spaces to work auxiliary verb as well //
                //////////////////////////////////////////////////////////////
                if (!translationInfoObject.TryParseMemberAndAdd(partOfSpeech.Replace(' ', '\0'), itemToken))
                {
#if DEBUG
                    //sometimes response contains members without name. Just ignore it.
                    Debug.WriteLineIf(partOfSpeech.Trim() != String.Empty,
                        $"class {typeof(T).Name} doesn't contains a member for a part " +
                        $"of speech {partOfSpeech}");
#endif
                }
            }

            return translationInfoObject;
        }

        private static string[] GetSeeAlso(JToken response)
        {
            return !response.HasValues ? new string[0] : response[0].ToObject<string[]>();
        }

        private static void GetMainTranslationInfo(JToken translationInfo, out string[] translate,
            ref string originalTextTranscription, ref string translatedTextTranscription)
        {
            List<string> translations = new List<string>();

            foreach (var item in translationInfo)
            {
                if (item.Count() >= 5)
                    translations.Add(item.First.Value<string>());
                else
                {
                    var transcriptionInfo = item;
                    int elementsCount = transcriptionInfo.Count();

                    if (elementsCount == 3)
                    {
                        translatedTextTranscription = (string)transcriptionInfo[elementsCount - 1];
                    }
                    else
                    {
                        if (transcriptionInfo[elementsCount - 2] != null)
                            translatedTextTranscription = (string)transcriptionInfo[elementsCount - 2];
                        else
                            translatedTextTranscription = (string)transcriptionInfo[elementsCount - 1];

                        originalTextTranscription = (string)transcriptionInfo[elementsCount - 1];
                    }
                }
            }

            translate = translations.ToArray();
        }

        private static Corrections GetTranslationCorrections(JToken response)
        {
            if (!response.HasValues)
                return new Corrections();

            Corrections corrections = new Corrections();

            JToken textCorrectionInfo = response[7];

            if (textCorrectionInfo.HasValues)
            {
                Regex pattern = new Regex(@"<b><i>(.*?)</i></b>");
                MatchCollection matches = pattern.Matches((string)textCorrectionInfo[0]);

                var correctedText = (string)textCorrectionInfo[1];
                var correctedWords = new string[matches.Count];

                for (int i = 0; i < matches.Count; i++)
                    correctedWords[i] = matches[i].Groups[1].Value;

                corrections.CorrectedWords = correctedWords;
                corrections.CorrectedText = correctedText;
                corrections.TextWasCorrected = true;
            }

            return corrections;
        }

        private static IEnumerable<Tuple<string,double>> GetLanguageDetections(JArray item)
        {
            JArray languages = item[0] as JArray;
            JArray confidences = item[2] as JArray;

            if (languages == null || confidences == null || languages.Count != confidences.Count)
                yield break;

            for (int i = 0; i < languages.Count; i++)
            {
                yield return Tuple.Create((string)languages[i], (double)confidences[i]);
            }
        }

        /// <summary>
        /// https://translate.google.com/m?hl=en&sl=en&tl=pt&ie=UTF-8&prev=_m&q=Text
        /// https://translate.google.com/?sl=en&tl=ta&op=translate&text=Dhuruva%20Natchathiram
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="toLanguage"></param>
        /// <param name="fromLanguage"></param>
        /// <returns></returns>
        public static async Task<TranslationResult> TranslateTextScrape(this string pThis, string fromLanguage = "en", string toLanguage = "ta")
        {
            TranslationResult translationResult = new TranslationResult();
            var url = $"https://translate.google.com/m?hl=en" +
                $"&sl={fromLanguage}&tl={toLanguage}&hl=en" +
                $"&ie=UTF-8&prev=_m" +
                $"&q={HttpUtility.UrlEncode(pThis)}";

            translationResult.FragmentedTranslation = new string[] { "" };
            var result = string.Empty;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.AcceptCharset.ParseAdd("UTF-8");
                try
                {
                    result = await httpClient.GetStringAsync(url);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(result);
                    var translatedNode = doc.DocumentNode.SelectSingleNode("//div[@class='result-container']");
                    translationResult.FragmentedTranslation = new string[] { translatedNode.InnerText };
                }
                catch (HttpRequestException ex) when (ex.Message.Contains("503"))
                {
                    throw new GoogleTranslateIPBannedException(GoogleTranslateIPBannedException.Operation.Translation);
                }
            }
            return translationResult;
        }
    }


}
