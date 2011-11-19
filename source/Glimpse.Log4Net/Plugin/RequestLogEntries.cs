using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Log4Net.Appender;
using log4net.Core;

namespace Glimpse.Log4Net.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    public class RequestLogEntries : IGlimpsePlugin
    {
        public const string ContextKey = "Glimpse.Log4Net.LogEntries";
        
        /// <summary>
        /// The maximum number of log entries to serialize to the client.
        /// Over this number only the main portions of the entry
        /// (level, timestamp, and message) are sent. 
        /// </summary>
        /// <remarks>
        /// 1,000 seems to be the emperical sweet spot for including 
        /// full log details before performance begins to suffer.
        /// If you have more than this and still want to see the details, 
        /// consider upping the appender's log level threshold.
        /// </remarks>
        public static volatile int MaxDetailedLogs = 1000;

        public string Name
        {
            get { return "log4net"; }
        }

        public void SetupInit()
        {
            GlimpseAppender.Initialize();
        }

        public object GetData(HttpContextBase context)
        {
            var logEntries = (IEnumerable<LoggingEvent>)context.Items[ContextKey];

            if (logEntries == null)
                return null;

            // Evaluate any expression trees
            logEntries = logEntries.ToArray();

            var data = new List<object[]>();

            // Only include the full log entry when there is a reasonable 
            // number of log messages
            var includeDetails = logEntries.Count() < MaxDetailedLogs;

            if (includeDetails)
            {
                data.Add(new[] { "Level", "Timestamp", "Message", "Log Entry" });
                data.AddRange(logEntries.Select(log => new object[] {
                                    log.Level.DisplayName, 
                                    log.TimeStamp, 
                                    log.RenderedMessage,
                                    log,
                                    GetStyle(log.Level),
                                }));
            }
            else
            {
                data.Add(new[] { "Level", "Timestamp", "Message" });
                data.AddRange(logEntries.Select(log => new object[] {
                                    log.Level.DisplayName, 
                                    log.TimeStamp, 
                                    log.RenderedMessage,
                                    GetStyle(log.Level),
                                }));
            }


            // Clean up after ourselves
            context.Items[ContextKey] = null;

            return data;
        }

        private string GetStyle(Level level)
        {
            var value = level.Value;

            if(value < Level.Info.Value)
                return "quiet";

            if(value >= Level.Info.Value && value < Level.Warn.Value)
                return "info";

            if(value >= Level.Warn.Value && value < Level.Error.Value)
                return "warn";

            if (value >= Level.Error.Value && value < Level.Alert.Value)
                return "error";

            if(value >= Level.Alert.Value)
                return "fail";

            return level.Name;
        }
    }
}