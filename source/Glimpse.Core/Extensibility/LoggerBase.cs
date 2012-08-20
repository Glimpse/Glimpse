using System;

namespace Glimpse.Core.Extensibility
{
    public abstract class LoggerBase:ILogger
    {
        public abstract void Trace(string message);
        public abstract void Debug(string message);
        public abstract void Info(string message);
        public abstract void Warn(string message);
        public abstract void Error(string message);
        public abstract void Fatal(string message);
        public abstract void Trace(string message, Exception exception);
        public abstract void Debug(string message, Exception exception);
        public abstract void Info(string message, Exception exception);
        public abstract void Warn(string message, Exception exception);
        public abstract void Error(string message, Exception exception);
        public abstract void Fatal(string message, Exception exception);

        public void Trace(string messageFormat, params object[] args)
        {
            Trace(string.Format(messageFormat, args));
        }

        public void Debug(string messageFormat, params object[] args)
        {
            Debug(string.Format(messageFormat, args));
        }

        public void Info(string messageFormat, params object[] args)
        {
            Info(string.Format(messageFormat, args));
        }

        public void Warn(string messageFormat, params object[] args)
        {
            Warn(string.Format(messageFormat, args));
        }

        public void Error(string messageFormat, params object[] args)
        {
            Error(string.Format(messageFormat, args));
        }

        public void Fatal(string messageFormat, params object[] args)
        {
            Fatal(string.Format(messageFormat, args));
        }

        public void Trace(string messageFormat, Exception exception, params object[] args)
        {
            Trace(string.Format(messageFormat, args), exception);
        }

        public void Debug(string messageFormat, Exception exception, params object[] args)
        {
            Debug(string.Format(messageFormat, args), exception);
        }

        public void Info(string messageFormat, Exception exception, params object[] args)
        {
            Info(string.Format(messageFormat, args), exception);
        }

        public void Warn(string messageFormat, Exception exception, params object[] args)
        {
            Warn(string.Format(messageFormat, args), exception);
        }

        public void Error(string messageFormat, Exception exception, params object[] args)
        {
            Error(string.Format(messageFormat, args), exception);
        }

        public void Fatal(string messageFormat, Exception exception, params object[] args)
        {
            Fatal(string.Format(messageFormat, args), exception);
        }
    }
}