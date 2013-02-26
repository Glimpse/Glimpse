﻿using System.Diagnostics;
﻿using Glimpse.Core.Message;
﻿using Glimpse.Core.Extensibility;
﻿using Glimpse.Core.Tab.Assist;
﻿using Glimpse.Test.Common;
﻿using Moq;
using Xunit.Extensions;
﻿using TraceListener = Glimpse.Core.TraceListener;

namespace Glimpse.Test.Core
{
    public class TraceListenerShould
    {
        [Theory, AutoMock]
        public void WriteObject(TraceListener sut, ITabSetupContext context)
        {
            sut.Write(new TestClass());

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("Message") && m.Category == null)));
        }

        [Theory, AutoMock]
        public void WriteString(TraceListener sut, ITabSetupContext context)
        {
            sut.Write("Message");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("Message") && m.Category == null)));
        }

        [Theory, AutoMock]
        public void WriteObjectCategory(TraceListener sut, ITabSetupContext context)
        {
            sut.Write(new TestClass(), "Warn");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("Warn: Message") && m.Category == FormattingKeywords.Warn)));
        }

        [Theory, AutoMock]
        public void WriteStringCategory(TraceListener sut, ITabSetupContext context)
        {
            sut.Write("Message", "Info");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("Info: Message") && m.Category == FormattingKeywords.Info)));
        }

        [Theory, AutoMock]
        public void WriteLineObject(TraceListener sut, ITabSetupContext context)
        {
            sut.WriteLine(new TestClass());

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("Message") && m.Category == null)));
        }

        [Theory, AutoMock]
        public void WriteLineString(TraceListener sut, ITabSetupContext context)
        {
            sut.WriteLine("Message");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("Message") && m.Category == null)));
        }

        [Theory, AutoMock]
        public void WriteLineObjectCategory(TraceListener sut, ITabSetupContext context)
        {
            sut.WriteLine(new TestClass(), "Loading");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("Loading: Message") && m.Category == FormattingKeywords.Loading)));
        }

        [Theory, AutoMock]
        public void WriteLineStringCategory(TraceListener sut, ITabSetupContext context)
        {
            sut.WriteLine("Message", "quiet");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("quiet: Message") && m.Category == FormattingKeywords.Quiet)));
        }

        [Theory, AutoMock]
        public void FailString(TraceListener sut, ITabSetupContext context)
        {
            sut.Fail("Message");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("Message") && m.Category == FormattingKeywords.Fail)));
        }

        [Theory, AutoMock]
        public void FailStringDetail(TraceListener sut, ITabSetupContext context)
        {
            sut.Fail("Message", "Detail");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("Message Detail") && m.Category == FormattingKeywords.Fail)));
        }

        [Theory, AutoMock]
        public void TraceData(TraceListener sut, ITabSetupContext context)
        {
            sut.TraceData(new TraceEventCache(), "TestSource", TraceEventType.Verbose, 123, new TestClass());

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("TestSource: 123: Message\r\n") && m.Category == null)));
        }

        [Theory, AutoMock]
        public void TraceDataCallstack(TraceListener sut, ITabSetupContext context)
        {
            sut.TraceOutputOptions = TraceOptions.Callstack;
            sut.TraceData(new TraceEventCache(), "TestSource", TraceEventType.Error, 123, new TestClass());

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.StartsWith("TestSource: 123: Message\r\nCallstack") && m.Category == FormattingKeywords.Error)));
        }

        [Theory, AutoMock]
        public void TraceDataTimestamp(TraceListener sut, ITabSetupContext context)
        {
            sut.TraceOutputOptions = TraceOptions.Timestamp;
            sut.TraceData(new TraceEventCache(), "TestSource", TraceEventType.Error, 123, new TestClass());

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.StartsWith("TestSource: 123: Message\r\nTimestamp") && m.Category == FormattingKeywords.Error)));
        }

        [Theory, AutoMock]
        public void TraceDataMultiple(TraceListener sut, ITabSetupContext context)
        {
            sut.TraceData(new TraceEventCache(), "TestSource", TraceEventType.Critical, 123, new TestClass(), new TestClass());

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("TestSource: 123: Message, Message\r\n") && m.Category == FormattingKeywords.Fail)));
        }

        [Theory, AutoMock]
        public void TraceEvent(TraceListener sut, ITabSetupContext context)
        {
            sut.TraceEvent(new TraceEventCache(), "TestSource", TraceEventType.Warning, 123, "Message");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("TestSource: 123: Message\r\n") && m.Category == FormattingKeywords.Warn)));
        }

        [Theory, AutoMock]
        public void TraceEventMultiple(TraceListener sut, ITabSetupContext context)
        {
            sut.TraceEvent(new TraceEventCache(), "TestSource", TraceEventType.Warning, 123, "Test {0} {1}", "Message", "Test");

            sut.MessageBroker.Verify(mb => mb.Publish(It.Is<TraceMessage>(m => m.Message.Equals("TestSource: 123: Test Message Test\r\n") && m.Category == FormattingKeywords.Warn)));
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
