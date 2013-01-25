using System.Configuration;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node for logging settings in Glimpse.
    /// </summary>
    /// <remarks>
    /// Glimpse logging is mostly used to diagnose problems with Glimpse itself. Logging is off by default, but can be configured to output log statements at various levels.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <logging level="Trace" />
    /// ]]>
    /// </code>
    /// </example>
    public class LoggingElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the logging level used by Glimpse.
        /// </summary>
        /// <value>
        /// The string representation of any valid <see cref="LoggingLevel"/>.
        /// </value>
        [ConfigurationProperty("level", DefaultValue = LoggingLevel.Off)]
        public LoggingLevel Level
        {
            get { return (LoggingLevel)base["level"]; }
            set { base["level"] = value; }
        }
    }
}