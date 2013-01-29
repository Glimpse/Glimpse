﻿using System.Collections.Generic;
﻿using System.Diagnostics;
﻿using Glimpse.AspNet;
﻿using Glimpse.AspNet.Model;
﻿using Glimpse.AspNet.PipelineInspector;
﻿using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
﻿using Glimpse.Core.Tab.Assist;
﻿using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.PipelineInspector
{
    public class TraceInspectorShould
    {
        [Theory, AutoMock]
        public void WriteObject(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.Write(new TestClass());

            Assert.Equal(1, list.Count);
            Assert.Equal("Message", list[0].Message);
            Assert.Equal(null, list[0].Category);
        }

        [Theory, AutoMock]
        public void WriteString(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.Write("New Message");

            Assert.Equal(1, list.Count);
            Assert.Equal("New Message", list[0].Message);
            Assert.Equal(null, list[0].Category);
        }

        [Theory, AutoMock]
        public void WriteObjectCategory(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.Write(new TestClass(), "Warn");

            Assert.Equal(1, list.Count);
            Assert.Equal("Warn: Message", list[0].Message);
            Assert.Equal(FormattingKeywords.Warn, list[0].Category);
        }

        [Theory, AutoMock]
        public void WriteStringCategory(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.Write("New Message", "Info");

            Assert.Equal(1, list.Count);
            Assert.Equal("Info: New Message", list[0].Message);
            Assert.Equal(FormattingKeywords.Info, list[0].Category);
        }

        [Theory, AutoMock]
        public void WriteLineObject(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.WriteLine(new TestClass());

            Assert.Equal(1, list.Count);
            Assert.Equal("Message", list[0].Message);
            Assert.Equal(null, list[0].Category);
        }

        [Theory, AutoMock]
        public void WriteLineString(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.WriteLine("New Message");

            Assert.Equal(1, list.Count);
            Assert.Equal("New Message", list[0].Message);
            Assert.Equal(null, list[0].Category);
        }

        [Theory, AutoMock]
        public void WriteLineObjectCategory(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.WriteLine(new TestClass(), "Loading");

            Assert.Equal(1, list.Count);
            Assert.Equal("Loading: Message", list[0].Message);
            Assert.Equal(FormattingKeywords.Loading, list[0].Category);
        }

        [Theory, AutoMock]
        public void WriteLineStringCategory(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.WriteLine("New Message", "quiet");

            Assert.Equal(1, list.Count);
            Assert.Equal("quiet: New Message", list[0].Message);
            Assert.Equal(FormattingKeywords.Quiet, list[0].Category);
        }

        [Theory, AutoMock]
        public void FailString(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.Fail("Message");

            Assert.Equal(1, list.Count);
            Assert.Equal("Message", list[0].Message);
            Assert.Equal(FormattingKeywords.Fail, list[0].Category);
        }

        [Theory, AutoMock]
        public void FailStringDetail(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.Fail("Message", "Detail");

            Assert.Equal(1, list.Count);
            Assert.Equal("Message Detail", list[0].Message);
            Assert.Equal(FormattingKeywords.Fail, list[0].Category);
        }

        [Theory, AutoMock]
        public void TraceData(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore); 
            traceInspector.TraceData(new TraceEventCache(), "TestSource", TraceEventType.Verbose, 123, new TestClass());

            Assert.Equal(1, list.Count);
            Assert.Equal("TestSource: 123: Message\r\n", list[0].Message);
            Assert.Equal(null, list[0].Category);
        }

        [Theory, AutoMock]
        public void TraceDataCallstack(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.TraceOutputOptions = TraceOptions.Callstack;
            traceInspector.TraceData(new TraceEventCache(), "TestSource", TraceEventType.Error, 123, new TestClass());

            Assert.Equal(1, list.Count);
            Assert.True(list[0].Message.StartsWith("TestSource: 123: Message\r\nCallstack"));
            Assert.Equal(FormattingKeywords.Error, list[0].Category);
        }

        [Theory, AutoMock]
        public void TraceDataTimestamp(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.TraceOutputOptions = TraceOptions.Timestamp;
            traceInspector.TraceData(new TraceEventCache(), "TestSource", TraceEventType.Error, 123, new TestClass());

            Assert.Equal(1, list.Count);
            Assert.True(list[0].Message.StartsWith("TestSource: 123: Message\r\nTimestamp"));
            Assert.Equal(FormattingKeywords.Error, list[0].Category);
        }

        [Theory, AutoMock]
        public void TraceDataMultiple(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.TraceData(new TraceEventCache(), "TestSource", TraceEventType.Critical, 123, new TestClass(), new TestClass());

            Assert.Equal(1, list.Count);
            Assert.Equal("TestSource: 123: Message, Message\r\n", list[0].Message);
            Assert.Equal(FormattingKeywords.Fail, list[0].Category);
        }

        [Theory, AutoMock]
        public void TraceEvent(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.TraceEvent(new TraceEventCache(), "TestSource", TraceEventType.Warning, 123, "Message");

            Assert.Equal(1, list.Count);
            Assert.Equal("TestSource: 123: Message\r\n", list[0].Message);
            Assert.Equal(FormattingKeywords.Warn, list[0].Category);
        }

        [Theory, AutoMock]
        public void TraceEventMultiple(ITabSetupContext context)
        {
            var list = SetupMessageStore(context);

            var traceInspector = new GlimpseTraceListener(context.GetTabStore);
            traceInspector.TraceEvent(new TraceEventCache(), "TestSource", TraceEventType.Warning, 123, "Test {0} {1}", "Message", "Test");

            Assert.Equal(1, list.Count);
            Assert.Equal("TestSource: 123: Test Message Test\r\n", list[0].Message);
            Assert.Equal(FormattingKeywords.Warn, list[0].Category);
        }

        private IList<TraceModel> SetupMessageStore(ITabSetupContext context)
        {
            var list = new List<TraceModel>();
            context.GetTabStore().Setup(x => x.Get(Glimpse.AspNet.Tab.Trace.TraceMessageStoreKey)).Returns(list);
            context.GetTabStore().Setup(x => x.Get(Glimpse.AspNet.Tab.Trace.FirstWatchStoreKey)).Returns(null);
            context.GetTabStore().Setup(x => x.Get(Glimpse.AspNet.Tab.Trace.LastWatchStoreKey)).Returns(null);

            return list;
        } 

        public class TestClass
        {
            public override string ToString()
            {
                return "Message";
            }
        }
    }
}
