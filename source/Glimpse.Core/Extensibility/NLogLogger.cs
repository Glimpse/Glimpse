using System;
using NLog;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An implementation of <see cref="ILogger"/> based on NLog.
    /// </summary>
    public class NLogLogger : LoggerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogger" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public NLogLogger(Logger logger)
        {
            Logger = logger;
        }

        private Logger Logger { get; set; }

        /// <summary>
        /// Log message at Trace level.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Trace(string message)
        {
            Logger.Trace(message);
        }

        /// <summary>
        /// Log message at Debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Debug(string message)
        {
            Logger.Debug(message);
        }

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Info(string message)
        {
            Logger.Info(message);
        }

        /// <summary>
        /// Log message at Warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Warn(string message)
        {
            Logger.Warn(message);
        }

        /// <summary>
        /// Log message at Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Error(string message)
        {
            Logger.Error(message);
        }

        /// <summary>
        /// Log message at Fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        /// <summary>
        /// Log message at Trace level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public override void Trace(string message, Exception exception)
        {
            Logger.TraceException(message, exception);
        }

        /// <summary>
        /// Log message at Debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public override void Debug(string message, Exception exception)
        {
            Logger.DebugException(message, exception);
        }

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public override void Info(string message, Exception exception)
        {
            Logger.InfoException(message, exception);
        }

        /// <summary>
        /// Log message at Warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public override void Warn(string message, Exception exception)
        {
            Logger.WarnException(message, exception);
        }

        /// <summary>
        /// Log message at Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public override void Error(string message, Exception exception)
        {
            Logger.ErrorException(message, exception);
        }

        /// <summary>
        /// Log message at Fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public override void Fatal(string message, Exception exception)
        {
            Logger.FatalException(message, exception);
        }
    }
}