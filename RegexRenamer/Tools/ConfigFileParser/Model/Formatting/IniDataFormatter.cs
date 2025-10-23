using ConfigFileParser.Model.Configuration;

namespace ConfigFileParser.Model.Formatting
{
    /// <summary>
    ///     Formats a IniData structure to an string
    /// </summary>
    public interface IniDataFormatter
    {
        /// <summary>
        ///     Produces an string given
        /// </summary>
        /// <returns>The data to string.</returns>
        /// <param name="iniData">Ini data.</param>
        string IniDataToString(ConfigData iniData);

        /// <summary>
        ///     Configuration used by this formatter when converting IniData
        ///     to an string
        /// </summary>
        IniParserConfiguration Configuration {get;set;}
    }
    
} 