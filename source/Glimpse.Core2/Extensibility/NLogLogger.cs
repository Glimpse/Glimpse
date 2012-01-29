using System;
using NLog;

namespace Glimpse.Core2.Extensibility
{
    public class NLogLogger:ILogger
    {
        private Logger Logger { get; set; }

        public NLogLogger(Logger logger)
        {
            Logger = logger;
        }

        public void Trace(string message)
        {
            Logger.Trace(message);
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Info(string message)
        {
            Logger.Info(message);
        }

        public void Warn(string message)
        {
            Logger.Warn(message);
        }

        public void Error(string message)
        {
            Logger.Error(message);
        }

        public void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        public void Trace(string message, Exception exception)
        {
            Logger.Trace(message, exception);
        }

        public void Debug(string message, Exception exception)
        {
            Logger.Debug(message, exception);
        }

        public void Info(string message, Exception exception)
        {
            Logger.Info(message, exception);
        }

        public void Warn(string message, Exception exception)
        {
            Logger.Warn(message, exception);
        }

        public void Error(string message, Exception exception)
        {
            Logger.Error(message, exception);
        }

        public void Fatal(string message, Exception exception)
        {
            Logger.Fatal(message, exception);
        }
    }
}