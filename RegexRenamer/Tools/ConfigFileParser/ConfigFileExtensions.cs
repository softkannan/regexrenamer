using ConfigFileParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace ConfigFileParser
{
    public static class ConfigFileExtensions
    {
        public static string GetValue(this KeyDataCollection pThis, string valueName, string defaultValue = "")
        {
            if (pThis == null)
                throw new ArgumentNullException(nameof(pThis));
            if (string.IsNullOrEmpty(valueName))
                throw new ArgumentException("Value name cannot be null or empty", nameof(valueName));
            if (pThis.ContainsKey(valueName))
                return pThis[valueName];
            return defaultValue;
        }


        public static T GetValue<T>(this KeyDataCollection pThis, string valueName, T defaultValue = default(T))
        {
            if (pThis == null)
                throw new ArgumentNullException(nameof(pThis));
            if (string.IsNullOrEmpty(valueName))
                throw new ArgumentException("Value name cannot be null or empty", nameof(valueName));
            if (pThis.ContainsKey(valueName))
            {
                var tempVal = pThis[valueName];
                try
                {
                    return (T)Convert.ChangeType(tempVal, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        public static void SetValue(this KeyDataCollection pThis, string valueName, string value)
        {
            if (pThis == null)
                throw new ArgumentNullException(nameof(pThis));
            if (string.IsNullOrEmpty(valueName))
                throw new ArgumentException("Value name cannot be null or empty", nameof(valueName));
            pThis[valueName] = value;
        }

        public static void SetValue<T>(this KeyDataCollection pThis, string valueName, T value)
        {
            if (pThis == null)
                throw new ArgumentNullException(nameof(pThis));
            if (string.IsNullOrEmpty(valueName))
                throw new ArgumentException("Value name cannot be null or empty", nameof(valueName));
            pThis[valueName] = Convert.ToString(value);
        }
    }
}
