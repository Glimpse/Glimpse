using System;
using System.IO;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Wraps an <see cref="ILogger" /> so that code depending on <see cref="ILogger" /> can already store a reference to an <see cref="ILogger"/>,
    /// which will be an instance of this class, which will wrap the real logger. The real logger will default to a <see cref="NullLogger" /> 
    /// in case the configured log level is set to <see cref="LoggingLevel.Off"/> or with a <see cref="NLog.Logger" /> for all other log levels.
    /// The real logger can then be replaced with a call to <see cref="SwitchLogger"/>.
    /// </summary>
    internal class LoggerWrapper : ILogger
    {
        private LoggingLevel ConfiguredLogLevel { get; set; }

        private ILogger Logger { get; set; }

        public LoggerWrapper(LoggingLevel logLevel, string logLocation)
        {
            ConfiguredLogLevel = logLevel;

            // use NullLogger if logging is disabled
            Logger = ConfiguredLogLevel == LoggingLevel.Off ? new NullLogger() : CreateDefaultLogger(logLevel, logLocation);
        }

        public void SwitchLogger(ILogger logger)
        {
            Guard.ArgumentNotNull("logger", logger);

            if (ConfiguredLogLevel == LoggingLevel.Off)
            {
                // we keep our NullLogger that was initially assigned
                return;
            }

            var currentLogger = Logger as IDisposable;
            Logger = logger;

            if (currentLogger != null)
            {
                try
                {
                    currentLogger.Dispose();
                }
                catch (Exception disposeException)
                {
                    Logger.Error("Failed disposing the previous logger", disposeException);
                }
            }
        }

        private static ILogger CreateDefaultLogger(LoggingLevel logLevel, string configuredLoggingPath)
        {
            if (string.IsNullOrEmpty(configuredLoggingPath))
            {
                configuredLoggingPath = "Glimpse.log";
            }

            // Root the path if it isn't already and add a filename if one isn't specified
            var loggingPath = Path.IsPathRooted(configuredLoggingPath) ? configuredLoggingPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuredLoggingPath);
            var logFilePath = string.IsNullOrEmpty(Path.GetExtension(loggingPath)) ? Path.Combine(loggingPath, "Glimpse.log") : loggingPath;

            var fileTarget = new FileTarget
            {
                FileName = logFilePath,
                Layout = "${longdate} | ${level:uppercase=true} | ${message} | ${exception:maxInnerExceptionLevel=5:format=type,message,stacktrace:separator=--:innerFormat=shortType,message,method:innerExceptionSeparator=>>}"
            };

            var asyncTarget = new AsyncTargetWrapper(fileTarget);
            var loggingConfiguration = new NLog.Config.LoggingConfiguration();
            loggingConfiguration.AddTarget("file", asyncTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.FromOrdinal((int)logLevel), asyncTarget));

            return new NLogLogger(new LogFactory(loggingConfiguration).GetLogger("Glimpse"));
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

        public void Trace(string messageFormat, params object[] args)
        {
            Logger.Trace(messageFormat, args);
        }

        public void Debug(string messageFormat, params object[] args)
        {
            Logger.Debug(messageFormat, args);
        }

        public void Info(string messageFormat, params object[] args)
        {
            Logger.Info(messageFormat, args);
        }

        public void Warn(string messageFormat, params object[] args)
        {
            Logger.Warn(messageFormat, args);
        }

        public void Error(string messageFormat, params object[] args)
        {
            Logger.Error(messageFormat, args);
        }

        public void Fatal(string messageFormat, params object[] args)
        {
            Logger.Fatal(messageFormat, args);
        }

        public void Trace(string messageFormat, Exception exception, params object[] args)
        {
            Logger.Trace(messageFormat, exception, args);
        }

        public void Debug(string messageFormat, Exception exception, params object[] args)
        {
            Logger.Debug(messageFormat, exception, args);
        }

        public void Info(string messageFormat, Exception exception, params object[] args)
        {
            Logger.Info(messageFormat, exception, args);
        }

        public void Warn(string messageFormat, Exception exception, params object[] args)
        {
            Logger.Warn(messageFormat, exception, args);
        }

        public void Error(string messageFormat, Exception exception, params object[] args)
        {
            Logger.Error(messageFormat, exception, args);
        }

        public void Fatal(string messageFormat, Exception exception, params object[] args)
        {
            Logger.Fatal(messageFormat, exception, args);
        }
    }
}
