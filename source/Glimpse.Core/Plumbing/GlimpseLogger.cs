using System;
using Glimpse.Core.Extensibility;
using NLog;

namespace Glimpse.Core.Plumbing
{
    public class GlimpseLogger:IGlimpseLogger
    {
        internal Logger Logger { get; set; }

        public GlimpseLogger(Logger logger)
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

        public void Trace(object obj)
        {
            Logger.Trace(obj);
        }

        public void Debug(object obj)
        {
            Logger.Debug(obj);
        }

        public void Info(object obj)
        {
            Logger.Info(obj);
        }

        public void Warn(object obj)
        {
            Logger.Warn(obj);
        }

        public void Error(object obj)
        {
            Logger.Error(obj);
        }

        public void Fatal(object obj)
        {
            Logger.Fatal(obj);
        }

        public void Trace(string format, params object[] args)
        {
            Logger.Trace(format, args);
        }

        public void Debug(string format, params object[] args)
        {
            Logger.Debug(format, args);
        }

        public void Info(string format, params object[] args)
        {
            Logger.Info(format, args);
        }

        public void Warn(string format, params object[] args)
        {
            Logger.Warn(format, args);
        }

        public void Error(string format, params object[] args)
        {
            Logger.Error(format, args);
        }

        public void Fatal(string format, params object[] args)
        {
            Logger.Fatal(format, args);
        }

        public void Trace(string message, Exception exception)
        {
            Logger.TraceException(message, exception);
        }

        public void Debug(string message, Exception exception)
        {
            Logger.DebugException(message, exception);
        }

        public void Info(string message, Exception exception)
        {
            Logger.InfoException(message, exception);
        }

        public void Warn(string message, Exception exception)
        {
            Logger.WarnException(message, exception);
        }

        public void Error(string message, Exception exception)
        {
            Logger.ErrorException(message, exception);
        }

        public void Fatal(string message, Exception exception)
        {
            Logger.FatalException(message, exception);
        }
    }
}
