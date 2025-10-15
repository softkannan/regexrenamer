using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Calbre
{
    public class ArgKeyAttribute : Attribute
    {
       public string Key { get; set; }

       public ArgKeyAttribute(string key)
            { this.Key = key; }

    }

    public class ArgValueAttribute : Attribute
    {
        public string Value { get; set; }

        public ArgValueAttribute(string value) { this.Value = value; }
    }
}
