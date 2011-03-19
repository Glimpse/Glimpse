using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Glimpse.Net
{
    public class GlimpseTraceListener : TraceListener
    {
        protected IList<object[]> Messages { get; set; }
        private const string DefaultCategory = "Info";

        public GlimpseTraceListener(IDictionary store)
        {
            Messages = new List<object[]>
                           {
                               new object[] {"Message", "Category"}
                           };
            store.Add(GlimpseConstants.TraceMessages, Messages);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            throw new NotImplementedException();
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            throw new NotImplementedException();
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            throw new NotImplementedException();
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
            Messages.Add(new object[]
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
            //base.TraceEvent(eventCache, source, eventType, id);
        }
    }
}