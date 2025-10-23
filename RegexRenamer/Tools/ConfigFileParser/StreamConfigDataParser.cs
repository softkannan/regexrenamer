using System;
using System.IO;
using ConfigFileParser.Model;
using ConfigFileParser.Parser;
using ConfigFileParser.Model.Formatting;

namespace ConfigFileParser
{

    /// <summary>
    ///     Represents an INI data parser for streams.
    /// </summary>
    public class StreamConfigDataParser
    {
        /// <summary>
        ///     This instance will handle ini data parsing and writing
        /// </summary>
        public ConfigDataParser Parser { get; protected set; }

        /// <summary>
        ///     Represents an INI data parser for streams.
        /// </summary>
        public StreamConfigDataParser() : this (new ConfigDataParser()) {}

        /// <summary>
        ///     Represents an INI data parser for streams.
        /// </summary>
        /// <param name="parser"></param>
        public StreamConfigDataParser(ConfigDataParser parser)
        {
            Parser = parser;
        }

        /// <summary>
        ///     Reads data in INI format from a stream.
        /// </summary>
        /// <param name="reader">Reader stream.</param>
        /// <returns>
        ///     And <see cref="ConfigData"/> instance with the readed ini data parsed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="reader"/> is <c>null</c>.
        /// </exception>
        public ConfigData ReadData(StreamReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            
            return Parser.Parse(reader.ReadToEnd());
        }

        /// <summary>
        ///     Writes the ini data to a stream.
        /// </summary>
        /// <param name="writer">A write stream where the ini data will be stored</param>
        /// <param name="iniData">An <see cref="ConfigData"/> instance.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="writer"/> is <c>null</c>.
        /// </exception>
        public void WriteData(StreamWriter writer, ConfigData iniData)
        {
            if (iniData == null)
                throw new ArgumentNullException("iniData");
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.Write(iniData.ToString());
        }

        
        /// <summary>
        ///     Writes the ini data to a stream.
        /// </summary>
        /// <param name="writer">A write stream where the ini data will be stored</param>
        /// <param name="iniData">An <see cref="ConfigData"/> instance.</param>
        /// <param name="formatter">Formaterr instance that controls how the ini data is transformed to a string</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="writer"/> is <c>null</c>.
        /// </exception>
        public void WriteData(StreamWriter writer, ConfigData iniData, IniDataFormatter formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException("formatter");
            if (iniData == null)
                throw new ArgumentNullException("iniData");
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.Write(iniData.ToString(formatter));
        }
    }
}
