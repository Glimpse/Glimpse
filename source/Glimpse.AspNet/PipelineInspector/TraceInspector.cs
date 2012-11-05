using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Plugin.Assist;

namespace Glimpse.AspNet.PipelineInspector
{
    public class TraceInspector : System.Diagnostics.TraceListener
    {
        private readonly ITabSetupContext _context;

        public TraceInspector(ITabSetupContext context)
        {
            _context = context;
        }

        private Stopwatch FirstWatch
        {
            get
            {
                var firstWatch = _context.GetTabStore().Get<Stopwatch>(Tab.Trace.FirstWatchStoreKey);
                if (firstWatch == null) 
                {
                    firstWatch = new Stopwatch();
                    _context.GetTabStore().Set(Tab.Trace.FirstWatchStoreKey, firstWatch);
                    firstWatch.Start();
                }

                return firstWatch;
            } 
        }

        private Stopwatch LastWatch
        {
            get
            {
                var lastWatch = _context.GetTabStore().Get<Stopwatch>(Tab.Trace.LastWatchStoreKey);
                if (lastWatch == null)
                {
                    lastWatch = new Stopwatch();
                    _context.GetTabStore().Set(Tab.Trace.LastWatchStoreKey, lastWatch);
                    lastWatch.Start();
                } 

                return lastWatch;
            } 
        }

        private IList<TraceModel> Messages
        {
            get
            {
                var messages = _context.GetTabStore().Get<IList<TraceModel>>(Tab.Trace.TraceMessageStoreKey);
                if (messages == null) 
                {
                    messages = new List<TraceModel>();
                    _context.GetTabStore().Set(Tab.Trace.TraceMessageStoreKey, messages); 
                }
                
                return messages;
            }
        }
         
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
            var typedCategory = DeriveCategory(category);
            if (typedCategory == FormattingKeywordEnum.None && !string.IsNullOrEmpty(category))
            {
                message = category + ": " + message;
            }

            InternalWrite(message, typedCategory);
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

            InternalWrite(failMessage.ToString(), FormattingKeywordEnum.Fail);
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

        private void InternalWrite(string message, FormattingKeywordEnum category)
        {
            var firstWatch = FirstWatch;
            var lastWatch = LastWatch;

            var model = new TraceModel();
            model.Category = category;
            model.Message = message;
            model.FromFirst = firstWatch.ElapsedTicks.ConvertNanosecondsToMilliseconds();
            model.FromLast = lastWatch.ElapsedTicks.ConvertNanosecondsToMilliseconds();
            model.IndentLevel = IndentLevel;

            lastWatch.Reset();
            lastWatch.Start();

            Messages.Add(model);
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

        private FormattingKeywordEnum DeriveCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return FormattingKeywordEnum.None;
            }

            switch (category.ToLower())
            {
                case "warning":
                case "warn":
                    return FormattingKeywordEnum.Warn;
                case "information":
                case "info":
                    return FormattingKeywordEnum.Info;
                case "error":
                    return FormattingKeywordEnum.Error;
                case "fail":
                    return FormattingKeywordEnum.Fail;
                case "quiet":
                    return FormattingKeywordEnum.Quiet;
                case "timing":
                case "loading":
                    return FormattingKeywordEnum.Loading;
                case "selected":
                    return FormattingKeywordEnum.Selected;
                case "aspx.page":
                    return FormattingKeywordEnum.System;
            }

            return FormattingKeywordEnum.None;
        }

        private FormattingKeywordEnum DeriveCategory(TraceEventType type)
        {
            switch (type)
            {
                case TraceEventType.Error:
                    return FormattingKeywordEnum.Error;
                case TraceEventType.Critical:
                    return FormattingKeywordEnum.Fail;
                case TraceEventType.Warning:
                    return FormattingKeywordEnum.Warn;
                case TraceEventType.Information:
                    return FormattingKeywordEnum.Info;
            }

            return FormattingKeywordEnum.None;
        } 
    }
}
