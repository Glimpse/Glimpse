using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Glimpse.Core.Configuration
{
    public class RegexElement : ConfigurationElement
    {
        [ConfigurationProperty("regex")]
        [TypeConverter(typeof(RegexConverter))]
        public Regex Regex
        {
            get { return (Regex)base["regex"]; }
            set { base["regex"] = value; }
        }
    }
}