using Glimpse.Core.Configuration;

namespace Glimpse.Core
{
    /// <summary>
    /// Contains the logging settings used by Glimpse which is mostly used to diagnose problems with Glimpse itself. 
    /// Logging is off by default, but can be configured to output log statements at various levels.
    /// </summary>
    public class LoggingSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingSettings" /> class
        /// </summary>
        /// <param name="level">The logging level, which defaults to <c>LoggingLevel.Off</c>.</param>
        /// <param name="logLocation">The file path to the Glimpse log file, which defaults to Glimpse.log</param>
        public LoggingSettings(LoggingLevel level = LoggingLevel.Off, string logLocation = "Glimpse.log")
        {
            Level = level;
            LogLocation = string.IsNullOrEmpty(logLocation) ? "Glimpse.log" : logLocation;
        }

        /// <summary>
        /// Gets the logging level used by Glimpse.
        /// </summary>
        /// <value>
        /// The string representation of any valid <see cref="LoggingLevel"/>.
        /// The default value is <c>LoggingLevel.Off</c>.
        /// </value>
        public LoggingLevel Level { get; private set; }

        /// <summary>
        /// Gets the file path to the Glimpse log file.
        /// </summary>
        /// <remarks>
        /// <c>LogLocation</c> is only written to if <c>Level</c> is not set to <c>Off</c>.
        /// </remarks>
        /// <value>
        /// The absolute or relative file path to the log file. 
        /// Relative paths are rooted from <c>AppDomain.CurrentDomain.BaseDirectory</c>.
        /// The default value is <c>Glimpse.log</c>.
        /// </value>
        public string LogLocation { get; private set; }
    }
}