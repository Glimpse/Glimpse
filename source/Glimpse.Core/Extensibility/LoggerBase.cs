using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An abstract base class which provides <see cref="ILogger"/> implementations with message formating abilities.
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        /// <summary>
        /// Log message at Trace level.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void Trace(string message);

        /// <summary>
        /// Log message at Debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void Debug(string message);

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void Info(string message);

        /// <summary>
        /// Log message at Warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void Warn(string message);

        /// <summary>
        /// Log message at Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void Error(string message);

        /// <summary>
        /// Log message at Fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void Fatal(string message);

        /// <summary>
        /// Log message at Trace level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public abstract void Trace(string message, Exception exception);

        /// <summary>
        /// Log message at Debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public abstract void Debug(string message, Exception exception);

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public abstract void Info(string message, Exception exception);

        /// <summary>
        /// Log message at Warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public abstract void Warn(string message, Exception exception);

        /// <summary>
        /// Log message at Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public abstract void Error(string message, Exception exception);

        /// <summary>
        /// Log message at Fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public abstract void Fatal(string message, Exception exception);

        /// <summary>
        /// Log message at Trace level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        public void Trace(string messageFormat, params object[] args)
        {
            Trace(string.Format(messageFormat, args));
        }

        /// <summary>
        /// Log message at Debug level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        public void Debug(string messageFormat, params object[] args)
        {
            Debug(string.Format(messageFormat, args));
        }

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        public void Info(string messageFormat, params object[] args)
        {
            Info(string.Format(messageFormat, args));
        }

        /// <summary>
        /// Log message at Warn level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        public void Warn(string messageFormat, params object[] args)
        {
            Warn(string.Format(messageFormat, args));
        }

        /// <summary>
        /// Log message at Error level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        public void Error(string messageFormat, params object[] args)
        {
            Error(string.Format(messageFormat, args));
        }

        /// <summary>
        /// Log message at Fatal level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        public void Fatal(string messageFormat, params object[] args)
        {
            Fatal(string.Format(messageFormat, args));
        }

        /// <summary>
        /// Log message at Trace level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        public void Trace(string messageFormat, Exception exception, params object[] args)
        {
            Trace(string.Format(messageFormat, args), exception);
        }

        /// <summary>
        /// Log message at Debug level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        public void Debug(string messageFormat, Exception exception, params object[] args)
        {
            Debug(string.Format(messageFormat, args), exception);
        }

        /// <summary>
        /// Log message at Info level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        public void Info(string messageFormat, Exception exception, params object[] args)
        {
            Info(string.Format(messageFormat, args), exception);
        }

        /// <summary>
        /// Log message at Warn level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        public void Warn(string messageFormat, Exception exception, params object[] args)
        {
            Warn(string.Format(messageFormat, args), exception);
        }

        /// <summary>
        /// Log message at Error level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        public void Error(string messageFormat, Exception exception, params object[] args)
        {
            Error(string.Format(messageFormat, args), exception);
        }

        /// <summary>
        /// Log message at Fatal level.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        public void Fatal(string messageFormat, Exception exception, params object[] args)
        {
            Fatal(string.Format(messageFormat, args), exception);
        }
    }
}