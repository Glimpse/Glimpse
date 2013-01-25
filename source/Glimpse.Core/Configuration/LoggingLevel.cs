namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// <c>LoggingLevel</c> defines the level of output detail to be written to the Glimpse log.
    /// </summary>
    public enum LoggingLevel
    {
        /// <summary>
        /// The <c>Trace</c> level is the most detailed level of output in Glimpse.
        /// </summary>
        Trace = 0,

        /// <summary>
        /// The <c>Debug</c> level contains debugging information, and is less detailed than trace.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// The <c>Info</c> level contains common information messages.
        /// </summary>
        Info = 2,

        /// <summary>
        /// The <c>Warn</c> level contains warning messages, typically for non-critical issues, which can be recovered or which are temporary failures.
        /// </summary>
        Warn = 3,

        /// <summary>
        /// The <c>Error</c> level contains error messages that Glimpse was able to recover from.
        /// </summary>
        Error = 4,

        /// <summary>
        /// The <c>Fatal</c> level contains error messages that Glimpse was unable to recover from.
        /// </summary>
        Fatal = 5,

        /// <summary>
        /// The <c>Off</c> level contains no messages. When the logging level is <c>Off</c>, Glimpse will not even create a log file.
        /// </summary>
        Off = 6
    }
}