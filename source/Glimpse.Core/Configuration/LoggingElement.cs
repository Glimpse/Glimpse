using System.Configuration;

namespace Glimpse.Core.Configuration
{
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <logging level="Trace" />
    /// ]]>
    /// </code>
    /// </example>
    public class LoggingElement : ConfigurationElement
    {
        [ConfigurationProperty("level", DefaultValue = LoggingLevel.Off)]
        public LoggingLevel Level
        {
            get { return (LoggingLevel)base["level"]; }
            set { base["level"] = value; }
        }

        [ConfigurationProperty("logLocation")]
        public string LogLocation
        {
            get { return (string)base["logLocation"]; }
            set { base["logLocation"] = value; }
        }
    }
}