using System;

namespace Glimpse.Core2.Extensibility
{
    public interface ILogger
    {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);
        void Trace(string message, Exception exception);
        void Debug(string message, Exception exception);
        void Info(string message, Exception exception);
        void Warn(string message, Exception exception);
        void Error(string message, Exception exception);
        void Fatal(string message, Exception exception);


        void Trace(string messageFormat, params object[] args);
        void Debug(string messageFormat, params object[] args);
        void Info(string messageFormat, params object[] args);
        void Warn(string messageFormat, params object[] args);
        void Error(string messageFormat, params object[] args);
        void Fatal(string messageFormat, params object[] args);
        void Trace(string messageFormat, Exception exception, params object[] args);
        void Debug(string messageFormat, Exception exception, params object[] args);
        void Info(string messageFormat, Exception exception, params object[] args);
        void Warn(string messageFormat, Exception exception, params object[] args);
        void Error(string messageFormat, Exception exception, params object[] args);
        void Fatal(string messageFormat, Exception exception, params object[] args);
    }
}