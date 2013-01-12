using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition of the internal logger that will used by the system.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log message at Trace level. 
        /// </summary>
        /// <param name="message">The message.</param>
        void Trace(string message);

        /// <summary>
        /// Log message at Debug level. 
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);

        /// <summary>
        /// Log message at Warn level. 
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(string message);

        /// <summary>
        /// Log message at Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);

        /// <summary>
        /// Log message at Fatal level. 
        /// </summary>
        /// <param name="message">The message.</param>
        void Fatal(string message);

        /// <summary>
        /// Log message at Trace level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Trace(string message, Exception exception);

        /// <summary>
        /// Log message at Debug level. 
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Debug(string message, Exception exception);

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Info(string message, Exception exception);

        /// <summary>
        /// Log message at Warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Warn(string message, Exception exception);

        /// <summary>
        /// Log message at Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Log message at Fatal level. 
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Fatal(string message, Exception exception);

        /// <summary>
        /// Log message at Trace level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        void Trace(string messageFormat, params object[] args);

        /// <summary>
        /// Log message at Debug level. 
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        void Debug(string messageFormat, params object[] args);

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        void Info(string messageFormat, params object[] args);

        /// <summary>
        /// Log message at Warn level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        void Warn(string messageFormat, params object[] args);

        /// <summary>
        /// Log message at Error level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        void Error(string messageFormat, params object[] args);

        /// <summary>
        /// Log message at Fatal level. 
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        void Fatal(string messageFormat, params object[] args);

        /// <summary>
        /// Log message at Trace level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Trace(string messageFormat, Exception exception, params object[] args);

        /// <summary>
        /// Log message at Debug level. 
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Debug(string messageFormat, Exception exception, params object[] args);

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Info(string messageFormat, Exception exception, params object[] args);

        /// <summary>
        /// Log message at Warn level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Warn(string messageFormat, Exception exception, params object[] args);

        /// <summary>
        /// Log message at Error level. 
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Error(string messageFormat, Exception exception, params object[] args);

        /// <summary>
        /// Log message at Fatal level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Fatal(string messageFormat, Exception exception, params object[] args);
    }
}