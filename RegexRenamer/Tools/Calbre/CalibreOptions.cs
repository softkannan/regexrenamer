using RegexRenamer.Tools.Calbre;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.Calbre;

public class CalibreOptions
{
    private string _cmdArgs = null;

    public override string ToString()
    {
        if (_cmdArgs == null)
        {
            _cmdArgs = string.Empty;
            var sb = new StringBuilder();
            // Enumurate the properties and read attributes to generate command line
            var type = GetType();
            foreach (var prop in type.GetProperties())
            {
                // Get custom attribute
                var argKeyAttributes = prop.GetCustomAttributes<ArgKeyAttribute>(true);
                var argKey = argKeyAttributes.FirstOrDefault()?.Key;
                var argEnumVal = prop.GetValue(this);
                var enumType = argEnumVal.GetType();
                var memberInfos = enumType.GetMember(argEnumVal.ToString());
                var enumValueMemberInfo = memberInfos
                     .FirstOrDefault(m => m.DeclaringType == enumType);
                var argValAttributes = enumValueMemberInfo?.GetCustomAttributes<ArgValueAttribute>(false);
                var argVal = argValAttributes.FirstOrDefault()?.Value;
                
                if(!string.IsNullOrEmpty(argVal))
                {
                    sb.Append($"{argKey}{argVal.Trim()} ");
                }
            }
            _cmdArgs += sb.ToString().Trim();
        }
        return _cmdArgs;
    }

}


public class CalibreOptionsFactory
{
    public static CalibreOptions CreateInputFileOptions(string fileExt)
    {
        CalibreOptions retVal = new CalibreOptions();
        switch (fileExt)
        {
            case ".txt":
                retVal = new TxtFileInputOptions();
                break;
            case ".epub":
                retVal = new EpubInputFileOptions();
                break;
            case ".pdf":
                retVal = new PDFInputFileOptions();
                break;
        }
        return retVal;
    }
}