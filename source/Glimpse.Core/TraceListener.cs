using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.Core
{
    public class TraceListener : System.Diagnostics.TraceListener
    {
        [ThreadStatic]
        private static Stopwatch fromLastWatch;
        private IMessageBroker messageBroker;

        // ReSharper disable UnusedMember.Global
        // These constructors used by .NET when TraceListener is set via web.config
        public TraceListener() : this(GlimpseConfiguration.GetConfiguredMessageBroker(), GlimpseConfiguration.GetConfiguredTimerStrategy())
        {
        }

        // This constructor is needed for users who configure web.config with <add name="myListener" type="Glimpse.AspNet.TraceListener" initializeData="XYZ"/>
        public TraceListener(string initializeData) : this(GlimpseConfiguration.GetConfiguredMessageBroker(), GlimpseConfiguration.GetConfiguredTimerStrategy())
        {
        }
        //// ReSharper restore UnusedMember.Global

        public TraceListener(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
        {
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
        }

        internal IMessageBroker MessageBroker 
        {
            get { return messageBroker ?? (messageBroker = GlimpseConfiguration.GetConfiguredMessageBroker()); }
            set { messageBroker = value; }
        }

        internal Func<IExecutionTimer> TimerStrategy { get; set; }

        public override void Write(object o)
        { 
            if (o == null)
            {
                return;
            } 

            Write(o.ToString());
        }

        public override void Write(string message)
        {
            WriteLine(message, null);
        }

        public override void Write(object o, string category)
        {
            if (category == null)
            {
                Write(o);
            }
            else
            {
                Write(o == null ? string.Empty : o.ToString(), category);
            }
        }

        public override void Write(string message, string category)
        { 
            if (category == null)
            {
                Write(message); 
            }
            else
            {
                WriteLine(message, category);
            }
        }

        public override void WriteLine(object o)
        { 
            WriteLine(o == null ? string.Empty : o.ToString());
        }

        public override void WriteLine(string message)
        {
            WriteLine(message, null);
        }

        public override void WriteLine(object o, string category)
        { 
            WriteLine(o == null ? string.Empty : o.ToString(), category);
        }

        public override void WriteLine(string message, string category)
        {
            var derivedCategory = DeriveCategory(category) ?? category;
            if (!string.IsNullOrEmpty(derivedCategory))
            {
                message = category + ": " + message;
            }

            InternalWrite(message, derivedCategory);
        }

        public override void Fail(string message)
        {
            Fail(message, string.Empty);
        }

        public override void Fail(string message, string detailMessage)
        {
            var failMessage = new StringBuilder(); 
            failMessage.Append(message);
            if (!string.IsNullOrEmpty(detailMessage))
            {
                failMessage.Append(" ");
                failMessage.Append(detailMessage);
            }

            InternalWrite(failMessage.ToString(), FormattingKeywords.Fail);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            var message = new StringBuilder();
            message.Append(WriteHeader(source, id));
                
            if (data != null)
            {
                message.AppendLine(data.ToString());
            }
             
            message.Append(WriteFooter(eventCache));

            InternalWrite(message.ToString(), DeriveCategory(eventType));
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            var message = new StringBuilder();
            message.Append(WriteHeader(source, id));

            var dataMessage = new StringBuilder();
            if (data != null)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    if (i != 0)
                    {
                        dataMessage.Append(", ");
                    }

                    if (data[i] != null)
                    {
                        dataMessage.Append(data[i]);
                    }
                }
            }

            message.AppendLine(dataMessage.ToString());
            message.Append(WriteFooter(eventCache));

            InternalWrite(message.ToString(), DeriveCategory(eventType));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string data)
        {
            var message = new StringBuilder(); 
            message.Append(WriteHeader(source, id));
            message.AppendLine(data);
            message.Append(WriteFooter(eventCache));

            InternalWrite(message.ToString(), DeriveCategory(eventType));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            var message = new StringBuilder();
            message.Append(WriteHeader(source, id));
            message.AppendLine(args != null ? string.Format(CultureInfo.InvariantCulture, format, args) : format);
            message.Append(WriteFooter(eventCache));

            InternalWrite(message.ToString(), DeriveCategory(eventType));
        }

        private TimeSpan CalculateFromLast(IExecutionTimer timer)
        {
            if (fromLastWatch == null)
            {
                fromLastWatch = Stopwatch.StartNew();
                return TimeSpan.FromMilliseconds(0);
            }

            // Timer started before this request, reset it
            if (DateTime.Now - fromLastWatch.Elapsed < timer.RequestStart)
            {
                fromLastWatch = Stopwatch.StartNew();
                return TimeSpan.FromMilliseconds(0);
            }

            var result = fromLastWatch.Elapsed;
            fromLastWatch = Stopwatch.StartNew();
            return result;
        }

        private void InternalWrite(string message, string category)
        {
            var timer = TimerStrategy();

            // Execution in on thread without access to RequestStore
            if (timer == null || MessageBroker == null) 
            {
                return;
            }

            var model = new TraceMessage
                {
                    Category = category,
                    Message = message,
                    FromFirst = timer.Point().Offset,
                    FromLast = CalculateFromLast(timer),
                    IndentLevel = IndentLevel
                };

            MessageBroker.Publish(model);
        }

        private string WriteHeader(string source, int id)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}: {1}: ", source, id.ToString(CultureInfo.InvariantCulture));
        }

        private string WriteFooter(TraceEventCache eventCache)
        {
            if (eventCache == null)
            {
                return string.Empty;
            }

            var message = new StringBuilder();
            if (IsEnabled(TraceOptions.ProcessId))
            {
                message.AppendLine("ProcessId=" + eventCache.ProcessId);
            }

            if (IsEnabled(TraceOptions.LogicalOperationStack))
            {
                message.Append("LogicalOperationStack=");
                var operationStack = eventCache.LogicalOperationStack;
                var first = true;
                foreach (var obj in operationStack)
                {
                    if (!first)
                    {
                        message.Append(", ");
                    }
                    else
                    {
                        first = false;
                    }

                    message.Append(obj);
                }

                message.AppendLine(string.Empty);
            }

            if (IsEnabled(TraceOptions.ThreadId))
            {
                message.AppendLine("ThreadId=" + eventCache.ThreadId);
            }

            if (IsEnabled(TraceOptions.DateTime))
            {
                message.AppendLine("DateTime=" + eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
            }

            if (IsEnabled(TraceOptions.Timestamp))
            {
                message.AppendLine("Timestamp=" + eventCache.Timestamp);
            }

            if (IsEnabled(TraceOptions.Callstack))
            {
                message.AppendLine("Callstack=" + eventCache.Callstack);
            }

            return message.ToString();
        }

        private bool IsEnabled(TraceOptions opts)
        {
            return (opts & TraceOutputOptions) != 0;
        }

        private string DeriveCategory(string category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                switch (category.ToLower())
                {
                    case "warning":
                    case "warn":
                        return FormattingKeywords.Warn;
                    case "information":
                    case "info":
                        return FormattingKeywords.Info;
                    case "error":
                        return FormattingKeywords.Error;
                    case "fail":
                        return FormattingKeywords.Fail;
                    case "quiet":
                        return FormattingKeywords.Quiet;
                    case "timing":
                    case "loading":
                        return FormattingKeywords.Loading;
                    case "selected":
                        return FormattingKeywords.Selected;
                    case "aspx.page":
                    case "system":
                    case "ms":
                        return FormattingKeywords.Ms;
                } 
            }

            return null;
        }

        private string DeriveCategory(TraceEventType type)
        {
            switch (type)
            {
                case TraceEventType.Error:
                    return FormattingKeywords.Error;
                case TraceEventType.Critical:
                    return FormattingKeywords.Fail;
                case TraceEventType.Warning:
                    return FormattingKeywords.Warn;
                case TraceEventType.Information:
                    return FormattingKeywords.Info;
            }

            return null;
        } 
    }
}
