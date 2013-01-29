using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node representing a regular expression.
    /// </summary>
    public class RegexElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the regular expression.
        /// </summary>
        /// <value>
        /// Any valid <see href="http://msdn.microsoft.com/en-us/library/hs600312%28v=vs.110%29.aspx">.NET Framework regular expression</see>. is supported. 
        /// </value>
        [ConfigurationProperty("regex")]
        [TypeConverter(typeof(RegexConverter))]
        public Regex Regex
        {
            get { return (Regex)base["regex"]; }
            set { base["regex"] = value; }
        }
    }
}