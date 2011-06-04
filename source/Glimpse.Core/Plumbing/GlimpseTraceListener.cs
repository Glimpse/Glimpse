using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Trace = Glimpse.Core.Plugin.Trace;

namespace Glimpse.Core.Plumbing
{
    public class GlimpseTraceListener : TraceListener
    {
        private static Stopwatch FirstWatch
        {
            get
            {
                var currentContext = HttpContext.Current; //Todo:Can we remove this somehow?
                Stopwatch firstWatch = null;

                if (currentContext != null)
                {
                    var store = currentContext.Items;
                    firstWatch = store[Trace.FirstWatchStoreKey] as Stopwatch;

                    if (firstWatch != null) return firstWatch;

                    store[Trace.FirstWatchStoreKey] = firstWatch = new Stopwatch();
                    firstWatch.Start();
                }
                return firstWatch;
            }
            set
            {
                var currentContext = HttpContext.Current;

                if (currentContext == null) return;

                currentContext.Items[Trace.FirstWatchStoreKey] = value;
                value.Start();
            }
        }

        private static Stopwatch LastWatch
        {
            get
            {
                var currentContext = HttpContext.Current;
                Stopwatch lastWatch = null;

                if (currentContext != null)
                {
                    var store = currentContext.Items;
                    lastWatch = store[Trace.LastWatchStoreKey] as Stopwatch;

                    if (lastWatch != null) return lastWatch;

                    store[Trace.LastWatchStoreKey] = lastWatch = new Stopwatch();
                    lastWatch.Start();
                }

                return lastWatch;
            }
            set
            {
                var currentContext = HttpContext.Current;

                if (currentContext == null) return;

                currentContext.Items[Trace.LastWatchStoreKey] = value;
                value.Start();
            }
        }

        private static IList<IList<string>> Messages
        {
            get
            {
                var currentContext = HttpContext.Current;
                IList<IList<string>> messages = null;

                if (currentContext != null)
                {
                    var store = currentContext.Items;
                    messages = store[Trace.TraceMessageStoreKey] as IList<IList<string>>;

                    if (messages != null) return messages;

                    store[Trace.TraceMessageStoreKey] = messages = new List<IList<string>>
                                                                       {
                                                                           new List<string>
                                                                               {
                                                                                   "Message",
                                                                                   "Category",
                                                                                   "From First",
                                                                                   "From Last"
                                                                               }
                                                                       };
                }

                return messages;
            }
        }

        private const string DefaultCategory = "Info";

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message,
                                           Guid relatedActivityId)
        {
            Write(message);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
                                       params object[] data)
        {
            Write(string.Format("Source: {0} Callstack: {1}", source, eventCache.Callstack), eventType.ToString());
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
                                       object data)
        {
            Write(string.Format("Source: {0} Callstack: {1}", source, eventCache.Callstack), eventType.ToString());
        }

        public override void Write(string message)
        {
            Write(message, DefaultCategory);
        }

        public override void WriteLine(string message)
        {
            Write(message);
        }

        public override void Fail(string message)
        {
            Write(message, "Fail");
        }

        public override void Fail(string message, string detailMessage)
        {
            Write(message + " " + detailMessage, "Fail");
        }

        public override void Write(object o)
        {
            Write(o, DefaultCategory);
        }

        public override void Write(string message, string category)
        {
            string elapsedSinceFirst = null;
            string elapsedSinceLast = null;

            if (FirstWatch != null)
                elapsedSinceFirst = FirstWatch.ElapsedMilliseconds + " ms";
            else
            {
                FirstWatch = new Stopwatch();
            }

            if (LastWatch != null)
            {
                elapsedSinceLast = LastWatch.ElapsedMilliseconds + " ms";
                LastWatch.Restart();
            }
            else
            {
                LastWatch = new Stopwatch();
            }

            if (Messages == null) return;

            var count = Messages.Count;

            Messages.Add(ApplyStyle(new List<string>
                                        {
                                            message,
                                            string.IsNullOrEmpty(category) ? "Info" : category,
                                            count == 1 ? null : elapsedSinceFirst,
                                            count == 1 ? null : elapsedSinceLast
                                        }));
        }

        public override void Write(object o, string category)
        {
            Write(o.ToString(), category);
        }

        protected override void WriteIndent()
        {
        }

        public override void WriteLine(object o)
        {
            Write(o);
        }

        public override void WriteLine(string message, string category)
        {
            Write(message, category);
        }

        public override void WriteLine(object o, string category)
        {
            Write(o, category);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
                                        string format, params object[] args)
        {
            Write(string.Format(format, args), eventType.ToString());
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
                                        string message)
        {
            Write(message, eventType.ToString());
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            Write(string.Format("Source: {0} Callstack: {1}", source, eventCache.Callstack), eventType.ToString());
        }

        private List<string> ApplyStyle(List<string> message)
        {
            switch (message[1].ToLower())
            {
                case "warning":
                case "warn":
                    message.Add("warn");
                    break;
                case "information":
                case "info":
                    message.Add("info");
                    break;
                case "error":
                    message.Add("error");
                    break;
                case "fail":
                    message.Add("fail");
                    break;
                case "quiet":
                    message.Add("quiet");
                    break;
                case "timing":
                    message.Add("loading");
                    break;
                case "selected":
                    message.Add("selected");
                    break;
                case "aspx.page":
                    message.Add("ms");
                    break;
            }

            return message;
        }
    }
}