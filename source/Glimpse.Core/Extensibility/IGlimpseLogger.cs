using System;

namespace Glimpse.Core.Extensibility
{
    public interface IGlimpseLogger
    {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);
        void Trace(object obj);
        void Debug(object obj);
        void Info(object obj);
        void Warn(object obj);
        void Error(object obj);
        void Fatal(object obj);
        void Trace(string format, params object[] args);
        void Debug(string format, params object[] args);
        void Info(string format, params object[] args);
        void Warn(string format, params object[] args);
        void Error(string format, params object[] args);
        void Fatal(string format, params object[] args);
        void Trace(string message, Exception exception);
        void Debug(string message, Exception exception);
        void Info(string message, Exception exception);
        void Warn(string message, Exception exception);
        void Error(string message, Exception exception);
        void Fatal(string message, Exception exception);
    }
}
