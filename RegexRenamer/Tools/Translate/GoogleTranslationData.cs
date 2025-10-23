using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace RegexRenamer.Tools.Translate;

[DataContract]
public sealed class Corrections
{
    [DataMember] public bool TextWasCorrected { get; internal set; }
    [DataMember] public string CorrectedText { get; internal set; }
    [DataMember] public string[] CorrectedWords { get; internal set; }

    internal Corrections() { }
}

[DataContract]
public sealed class Definitions : TranslationInfoParser
{
    [DataContract]
    public sealed class Definition
    {
        [DataMember] public string Explanation { get; private set; }
        [DataMember] public string Example { get; private set; }

        internal Definition(string explantion, string example)
        {
            Explanation = explantion;
            Example = example;
        }

        public override string ToString() => $"Explantion: {Explanation} Example: {Example}";
    }

    [DataMember] public Definition[] Noun { get; internal set; }
    [DataMember] public Definition[] Verb { get; internal set; }
    [DataMember] public Definition[] Exclamation { get; internal set; }
    [DataMember] public Definition[] Adjective { get; internal set; }
    [DataMember] public Definition[] Adverb { get; internal set; }
    [DataMember] public Definition[] Abbreviation { get; internal set; }
    [DataMember] public Definition[] Article { get; internal set; }
    [DataMember] public Definition[] Preposition { get; internal set; }
    [DataMember] public Definition[] Suffix { get; internal set; }
    [DataMember] public Definition[] Conjunction { get; internal set; }
    [DataMember] public Definition[] Pronoun { get; internal set; }
    [DataMember] public Definition[] Prefix { get; internal set; }
    [DataMember] public Definition[] Symbol { get; internal set; }
    [DataMember] public Definition[] Contraction { get; internal set; }


    public override string ToString()
    {
        string info = String.Empty;

        info += FormatOutput(Noun, nameof(Noun));
        info += FormatOutput(Verb, nameof(Verb));
        info += FormatOutput(Pronoun, nameof(Pronoun));
        info += FormatOutput(Adverb, nameof(Adverb));
        info += FormatOutput(Adjective, nameof(Adjective));
        info += FormatOutput(Conjunction, nameof(Conjunction));
        info += FormatOutput(Preposition, nameof(Preposition));
        info += FormatOutput(Exclamation, nameof(Exclamation));
        info += FormatOutput(Suffix, nameof(Suffix));
        info += FormatOutput(Prefix, nameof(Prefix));
        info += FormatOutput(Contraction, nameof(Contraction));
        info += FormatOutput(Abbreviation, nameof(Abbreviation));
        info += FormatOutput(Symbol, nameof(Symbol));
        info += FormatOutput(Article, nameof(Article));

        return info.Trim();
    }

    private string FormatOutput(IEnumerable<Definition> formatData, string partOfSpeechName)
    {
        if (formatData == null || !formatData.Any())
            return String.Empty;

        int i = 1;
        string tmp = '\n' + partOfSpeechName + ':';
        return formatData.Aggregate(
            tmp, (current, definition) => current + ($"\n{i++}) " + definition.ToString()));
    }


    internal override bool TryParseMemberAndAdd(string memberName, JToken parseInformation)
    {
        PropertyInfo property = this.GetType().GetRuntimeProperty(memberName.ToCamelCase());
        if (property == null)
            return false;

        var definitions = (
            from definitionUnformatted in parseInformation
            let explantion = (string)definitionUnformatted[0]
            let example = (string)definitionUnformatted[definitionUnformatted.Count() - 1]
            select new Definition(explantion, example)
            ).ToArray();

        property.SetMethod.Invoke(this, new object[] { definitions });

        return true;
    }

    internal override int ItemDataIndex => 1;
}


[DataContract]
public sealed class ExtraTranslations : TranslationInfoParser
{
    [DataContract]
    public sealed class ExtraTranslation
    {
        [DataMember] public string Phrase { get; private set; }
        [DataMember] public string[] PhraseTranslations { get; private set; }

        internal ExtraTranslation(string phrase, string[] phraseTranslations)
        {
            Phrase = phrase;
            PhraseTranslations = phraseTranslations;
        }

        public override string ToString() => $"{Phrase}: {String.Join(", ", PhraseTranslations)}";
    }

    [DataMember] public ExtraTranslation[] Noun { get; internal set; }
    [DataMember] public ExtraTranslation[] Verb { get; internal set; }
    [DataMember] public ExtraTranslation[] Pronoun { get; internal set; }
    [DataMember] public ExtraTranslation[] Adverb { get; internal set; }
    [DataMember] public ExtraTranslation[] AuxiliaryVerb { get; internal set; }
    [DataMember] public ExtraTranslation[] Adjective { get; internal set; }
    [DataMember] public ExtraTranslation[] Conjunction { get; internal set; }
    [DataMember] public ExtraTranslation[] Preposition { get; internal set; }
    [DataMember] public ExtraTranslation[] Interjection { get; internal set; }
    [DataMember] public ExtraTranslation[] Suffix { get; internal set; }
    [DataMember] public ExtraTranslation[] Prefix { get; internal set; }
    [DataMember] public ExtraTranslation[] Abbreviation { get; internal set; }
    [DataMember] public ExtraTranslation[] Particle { get; internal set; }
    [DataMember] public ExtraTranslation[] Phrase { get; internal set; }

    public ExtraTranslations() { }


    private string FormatOutput(IEnumerable<ExtraTranslation> formatData, string partOfSpeechName)
    {
        if (formatData == null)
            return String.Empty;

        string result = partOfSpeechName + ":\n";

        return formatData.Aggregate(result, (current, data)
            => current + $"{data.Phrase}: {String.Join(", ", data.PhraseTranslations)}\n");
    }

    public override string ToString()
    {
        string result = String.Empty;

        result += FormatOutput(Noun, nameof(Noun));
        result += FormatOutput(Verb, nameof(Verb));
        result += FormatOutput(Pronoun, nameof(Pronoun));
        result += FormatOutput(Adverb, nameof(Adverb));
        result += FormatOutput(AuxiliaryVerb, nameof(AuxiliaryVerb));
        result += FormatOutput(Adjective, nameof(Adjective));
        result += FormatOutput(Conjunction, nameof(Conjunction));
        result += FormatOutput(Preposition, nameof(Preposition));
        result += FormatOutput(Interjection, nameof(Interjection));
        result += FormatOutput(Suffix, nameof(Suffix));
        result += FormatOutput(Prefix, nameof(Prefix));
        result += FormatOutput(Abbreviation, nameof(Abbreviation));
        result += FormatOutput(Particle, nameof(Particle));
        result += FormatOutput(Phrase, nameof(Phrase));

        return result.Trim();
    }


    internal override bool TryParseMemberAndAdd(string memberName, JToken parseInformation)
    {
        PropertyInfo property = this.GetType().GetRuntimeProperty(memberName.ToCamelCase());
        if (property == null)
            return false;

        var extraTranslations = new ExtraTranslation[parseInformation.Count()];

        for (int i = 0; i < parseInformation.Count(); i++)
            extraTranslations[i] = new ExtraTranslation(
                (string)parseInformation[i][0], parseInformation[i][1].ToObject<string[]>());

        property.SetMethod.Invoke(this,
            new object[] { extraTranslations });

        return true;
    }

    internal override int ItemDataIndex => 2;
}

[DataContract]
public sealed class LanguageDetection
{
    [DataMember]
    public Language Language { get; internal set; }
    [DataMember(EmitDefaultValue = false)]
    public double Confidence { get; internal set; }

    public LanguageDetection(Language language, double confidence)
    {
        Language = language;
        Confidence = confidence;
    }

    public override string ToString()
    {
        return $"{Language.FullName} ({Confidence:F5})";
    }
}

[DataContract]
public sealed class Synonyms : TranslationInfoParser
{
    [DataMember] public string[] Noun { get; internal set; }
    [DataMember] public string[] Exclamation { get; internal set; }
    [DataMember] public string[] Adjective { get; internal set; }
    [DataMember] public string[] Verb { get; internal set; }
    [DataMember] public string[] Adverb { get; internal set; }
    [DataMember] public string[] Preposition { get; internal set; }
    [DataMember] public string[] Conjunction { get; internal set; }
    [DataMember] public string[] Pronoun { get; internal set; }

    internal Synonyms() { }

    public override string ToString()
    {
        string info = String.Empty;
        info += FormatOutput(Noun, nameof(Noun));
        info += FormatOutput(Verb, nameof(Verb));
        info += FormatOutput(Pronoun, nameof(Pronoun));
        info += FormatOutput(Adverb, nameof(Adverb));
        info += FormatOutput(Adjective, nameof(Adjective));
        info += FormatOutput(Conjunction, nameof(Conjunction));
        info += FormatOutput(Preposition, nameof(Preposition));
        info += FormatOutput(Exclamation, nameof(Exclamation));

        return info.TrimEnd();
    }

    private string FormatOutput(IEnumerable<string> partOfSpeechData, string partOfSpeechName)
    {
        if (partOfSpeechData == null)
            return String.Empty;

        return !partOfSpeechData.Any()
            ? String.Empty
            : $"{partOfSpeechName}: {string.Join(", ", partOfSpeechData)} \n";
    }

    internal override bool TryParseMemberAndAdd(string memberName, JToken parseInformation)
    {
        PropertyInfo property = GetType().GetRuntimeProperty(memberName.ToCamelCase());
        if (property == null)
            return false;

        List<string> synonyms = new List<string>();
        foreach (var synonymsSet in parseInformation)
            synonyms.AddRange(synonymsSet[0].ToObject<string[]>());

        property.SetMethod.Invoke(this, new object[] { synonyms.ToArray() });

        return true;
    }

    internal override int ItemDataIndex => 1;
}

public abstract class TranslationInfoParser
{
    internal abstract bool TryParseMemberAndAdd(string memberName, JToken parseInformation);
    internal abstract int ItemDataIndex { get; }
    ////////////////////////////////////////////////////////////////////////////////////////////
    //I've created a method, because the where: new() statement requires a public constructor///
    ////////////////////////////////////////////////////////////////////////////////////////////    
    internal static T Create<T>() where T : TranslationInfoParser
    {
        Type type = typeof(T);

        if (type == typeof(ExtraTranslations))
            return new ExtraTranslations() as T;
        if (type == typeof(Definitions))
            return new Definitions() as T;
        if (type == typeof(Synonyms))
            return new Synonyms() as T;

        throw new NotSupportedException("type");
    }
}


[DataContract]
public partial class Language
{
    /// <summary>
    /// Language name ( doesn't affect anything )
    /// </summary>
    [DataMember]
    public string FullName { get; }

    /// <summary>
    /// ISO639  table: <see href="http://stnsoft.com/Muxman/mxp/ISO_639.html"/>
    /// </summary>
    [DataMember]
    public string ISO639 { get; }

    public static Language Auto { get; private set; }

    static Language()
    {
        Auto = new Language("Automatic", "auto");
    }

    /// <summary>
    /// Creates new Language
    /// </summary>
    /// <param name="fullName">Language full name (set what do you want) </param>
    /// <param name="iso639">ISO639 value</param>
    public Language(string fullName, string iso639)
    {
        FullName = fullName;
        ISO639 = iso639;
    }

    protected bool Equals(Language other)
    {
        return string.Equals(ISO639, other.ISO639, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals((Language)obj);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(ISO639);
    }

    public override string ToString()
    {
        return $"FullName: {FullName}, ISO639: {ISO639}";
    }
}