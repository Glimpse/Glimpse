using System;
using NLog;

namespace Glimpse.Core.Extensibility
{
    public class NLogLogger:LoggerBase
    {
        private Logger Logger { get; set; }

        public NLogLogger(Logger logger)
        {
            Logger = logger;
        }

        public override void Trace(string message)
        {
            Logger.Trace(message);
        }

        public override void Debug(string message)
        {
            Logger.Debug(message);
        }

        public override void Info(string message)
        {
            Logger.Info(message);
        }

        public override void Warn(string message)
        {
            Logger.Warn(message);
        }

        public override void Error(string message)
        {
            Logger.Error(message);
        }

        public override void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        public override void Trace(string message, Exception exception)
        {
            Logger.TraceException(message, exception);
        }

        public override void Debug(string message, Exception exception)
        {
            Logger.DebugException(message, exception);
        }

        public override void Info(string message, Exception exception)
        {
            Logger.InfoException(message, exception);
        }

        public override void Warn(string message, Exception exception)
        {
            Logger.WarnException(message, exception);
        }

        public override void Error(string message, Exception exception)
        {
            Logger.ErrorException(message, exception);
        }

        public override void Fatal(string message, Exception exception)
        {
            Logger.FatalException(message, exception);
        }
    }
}