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
    /// <summary>
    /// Listener that Glimpse can use to tap into triggered events.
    /// </summary>
    public class TraceListener : System.Diagnostics.TraceListener
    {
        [ThreadStatic]
        private static Stopwatch fromLastWatch;
        private IMessageBroker messageBroker;
        private Func<IExecutionTimer> timerStrategy;

        // ReSharper disable UnusedMember.Global 

        /// <summary>
        /// These constructors used by .NET when TraceListener is set via web.config
        /// </summary>
        public TraceListener()
        {
        }

        /// <summary>
        /// This constructor is needed for users who configure web.config with <add name="myListener" type="Glimpse.AspNet.TraceListener" initializeData="something"/>
        /// </summary>
        /// <param name="initializeData">Initialize data string</param>
        public TraceListener(string initializeData)
        {
        }

        //// ReSharper restore UnusedMember.Global

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceListener"/> class.
        /// </summary>
        /// <param name="messageBroker">The message broker.</param>
        /// <param name="timerStrategy">The timer strategy.</param>
        public TraceListener(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
        {
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
        }

#warning CGI: this is kinda dirty to have internal access purely for testing purposes
        internal IMessageBroker MessageBroker
        {
            get
            {
                return messageBroker ?? (messageBroker = GlimpseRuntime.IsAvailable ? GlimpseRuntime.Instance.Configuration.MessageBroker : null);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                messageBroker = value;
            }
        }

        private Func<IExecutionTimer> TimerStrategy
        {
            get
            {
                if (timerStrategy == null)
                {
                    if (GlimpseRuntime.IsAvailable)
                    {
                        timerStrategy = () =>
                        {
                            var currentRequestContext = GlimpseRuntime.Instance.CurrentRequestContext;

                            return currentRequestContext.CurrentRuntimePolicy != RuntimePolicy.Off
                                ? currentRequestContext.CurrentExecutionTimer
                                : null;
                        };
                    }
                }

                return timerStrategy;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                timerStrategy = value;
            }
        }

        /// <summary>
        /// Writes the value of the object's <see cref="M:System.Object.ToString" /> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object" /> whose fully qualified class name you want to write.</param>
        public override void Write(object o)
        {
            if (o == null)
            {
                return;
            }

            Write(o.ToString());
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            WriteLine(message, null);
        }

        /// <summary>
        /// Writes a category name and the value of the object's <see cref="M:System.Object.ToString" /> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object" /> whose fully qualified class name you want to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
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

        /// <summary>
        /// Writes a category name and a message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
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

        /// <summary>
        /// Writes the value of the object's <see cref="M:System.Object.ToString" /> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class, followed by a line terminator.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object" /> whose fully qualified class name you want to write.</param>
        public override void WriteLine(object o)
        {
            WriteLine(o == null ? string.Empty : o.ToString());
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            WriteLine(message, null);
        }

        /// <summary>
        /// Writes a category name and the value of the object's <see cref="M:System.Object.ToString" /> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class, followed by a line terminator.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object" /> whose fully qualified class name you want to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public override void WriteLine(object o, string category)
        {
            WriteLine(o == null ? string.Empty : o.ToString(), category);
        }

        /// <summary>
        /// Writes a category name and a message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public override void WriteLine(string message, string category)
        {
            var derivedCategory = DeriveCategory(category) ?? category;
            if (!string.IsNullOrEmpty(derivedCategory))
            {
                message = category + ": " + message;
            }

            InternalWrite(message, derivedCategory);
        }

        /// <summary>
        /// Emits an error message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="message">A message to emit.</param>
        public override void Fail(string message)
        {
            Fail(message, string.Empty);
        }

        /// <summary>
        /// Emits an error message and a detailed error message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="message">A message to emit.</param>
        /// <param name="detailMessage">A detailed message to emit.</param>
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

        /// <summary>
        /// Writes trace information, a data object and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The trace data to emit.</param> 
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

        /// <summary>
        /// Writes trace information, an array of data objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">An array of objects to emit as data.</param> 
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

        /// <summary>
        /// Traces the event.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">The id.</param>
        /// <param name="data">The data.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string data)
        {
            var message = new StringBuilder();
            message.Append(WriteHeader(source, id));
            message.AppendLine(data);
            message.Append(WriteFooter(eventCache));

            InternalWrite(message.ToString(), DeriveCategory(eventType));
        }

        /// <summary>
        /// Writes trace information, a formatted array of objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args" /> array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param> 
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
            if (MessageBroker == null || TimerStrategy == null)
            {
                return;
            }

            var timer = TimerStrategy();

            if (timer == null)
            {
                // it can still be null in case the timer strategy decides to not return a timer (RuntimePolicy == Off for instance)
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
