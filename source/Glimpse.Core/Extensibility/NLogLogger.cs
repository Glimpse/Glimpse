using System;
using System.Threading;
using NLog;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An implementation of <see cref="ILogger"/> based on NLog.
    /// </summary>
    public class NLogLogger : LoggerBase, IDisposable
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

        public void Dispose()
        {
            NLog.LogManager.Flush(100);
            
            // NLog writes its logs asynchronously, which means that if we don't pause the thread, chances are the log will not be written 
            // especially if dispose is called on appDomain unload. Therefore we pause the thread for 100ms which should be enough for NLog
            // to do its thing, as we only give it 100ms to start with
            Thread.Sleep(110);
        }
    }
}