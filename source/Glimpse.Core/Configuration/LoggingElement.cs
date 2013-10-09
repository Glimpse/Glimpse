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

        /// <summary>
        /// Gets or sets the file path to the Glimpse log file.
        /// </summary>
        /// <remarks>
        /// <c>LogLocation</c> is only written to if <c>Level</c> is not set to <c>Off</c>.
        /// </remarks>
        /// <value>
        /// The absolute or relative file path to the log file. 
        /// Relative paths are rooted from <c>AppDomain.CurrentDomain.BaseDirectory</c>.
        /// The default value is <c>Glimpse.log</c>.
        /// </value>
        [ConfigurationProperty("logLocation", DefaultValue = "Glimpse.log")]
        public string LogLocation
        {
            get { return (string)base["logLocation"]; }
            set { base["logLocation"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether request flow handling logs should be written or not.
        /// </summary>
        /// <remarks>
        /// Even if the value is <c>true</c> then the log messages will only appear in the log if logging is enabled with at least a <c>LogLevel</c> of <c>Debug</c>
        /// </remarks>
        /// <value>
        /// Value indicating whether request flow handling logs should be written or not.
        /// </value>
        [ConfigurationProperty("writeRequestFlowHandlingLogs", DefaultValue = false)]
        public bool WriteRequestFlowHandlingLogs
        {
            get { return (bool)base["writeRequestFlowHandlingLogs"]; }
            set { base["writeRequestFlowHandlingLogs"] = value; }
        }
    }
}