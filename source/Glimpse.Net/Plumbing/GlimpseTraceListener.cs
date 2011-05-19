using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;

namespace Glimpse.WebForms.Plumbing
{
    public class GlimpseTraceListener : TraceListener
    {
        protected IList<IList<string>> Messages { 
            get
            {
                var store = HttpContext.Current.Items;
                var messages = store[Plugin.Trace.TraceMessageStoreKey] as IList<IList<string>>;

                if (messages != null) return messages;

                store[Plugin.Trace.TraceMessageStoreKey] = messages = new List<IList<string>>{
                                                                                              new List<string> {"Message", "Category"}
                                                                                          };

                return messages;
            }
            set { HttpContext.Current.Items[Plugin.Trace.TraceMessageStoreKey] = value; }
        }
        private const string DefaultCategory = "Info";

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            Write(message);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            Write(string.Format("Source: {0} Callstack: {1}", source, eventCache.Callstack), eventType.ToString());
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
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
            Messages.Add(new List<string>
                             {
                                 message, category
                             }
                );
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
    }
}