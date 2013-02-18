using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Model;
using Glimpse.Core.Tab;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.Tab
{
    public class TimelineShould
    {
        [Fact]
        public void HaveProperContextObjectType()
        {
            var timeline = new Timeline();

            Assert.Null(timeline.RequestContextType);
        }

        [Fact]
        public void UseDefaultLifeCycleSupport()
        {
            var timeline = new Timeline();
            Assert.Equal(RuntimeEvent.EndRequest, timeline.ExecuteOn);
        }

        [Fact]
        public void BeNamedTrace()
        {
            var timeline = new Timeline();
            Assert.Equal("Timeline", timeline.Name);
        }

        [Fact]
        public void HaveADocumentationUri()
        {
            var timeline = new Timeline();

            Assert.False(string.IsNullOrWhiteSpace(timeline.DocumentationUri));
        }
         
        [Theory, AutoMock]
        public void ReturnData(ITabContext context)
        { 
            context.TabStore.Setup(x => x.Contains(typeof(IList<Glimpse.Core.Message.ITimelineMessage>).AssemblyQualifiedName)).Returns(true);
            context.TabStore.Setup(x => x.Get(typeof(IList<Glimpse.Core.Message.ITimelineMessage>).AssemblyQualifiedName)).Returns(BuildMessages());

            var timeline = new Timeline();
            var result = timeline.GetData(context) as TimelineModel;

            Assert.NotNull(result);
            Assert.Equal(TimeSpan.FromMilliseconds(7), result.Duration);
            Assert.Equal(3, result.Events.Count);
            Assert.Equal("TestName1", result.Events[0].Title);
            Assert.Equal("TestName3", result.Events[1].Title);
            Assert.Equal("TestName2", result.Events[2].Title);
        }

        [Theory, AutoMock]
        public void ReturnEmptyWhenNoData(ITabContext context)
        {
            context.TabStore.Setup(x => x.Get(typeof(Glimpse.Core.Message.ITimelineMessage).FullName)).Returns((IEnumerable<Glimpse.Core.Message.ITimelineMessage>)null);

            var timeline = new Timeline();
            var result = timeline.GetData(context) as TimelineModel;

            Assert.NotNull(result);
            Assert.Equal(TimeSpan.FromMilliseconds(0), result.Duration);
            Assert.NotNull(result.Events);
        }

        private IEnumerable<Glimpse.Core.Message.ITimelineMessage> BuildMessages()
        {
            return new List<Glimpse.Core.Message.ITimelineMessage>
                               {
                                   new TestTimelineMessage { Duration = TimeSpan.FromMilliseconds(1), EventCategory = Glimpse.Core.Message.TimelineMessage.Request, EventName = "TestName1", EventSubText = "TestSub1", Offset = TimeSpan.FromMilliseconds(1), StartTime = DateTime.Now },
                                   new TestTimelineMessage { Duration = TimeSpan.FromMilliseconds(4), EventCategory = Glimpse.Core.Message.TimelineMessage.Other, EventName = "TestName2", EventSubText = "TestSub2", Offset = TimeSpan.FromMilliseconds(3), StartTime = DateTime.Now },
                                   new TestTimelineMessage { Duration = TimeSpan.FromMilliseconds(1), EventCategory = Glimpse.Core.Message.TimelineMessage.Request, EventName = "TestName3", EventSubText = "TestSub3", Offset = TimeSpan.FromMilliseconds(2), StartTime = DateTime.Now }
                               };
        }

        public class TestTimelineMessage : Glimpse.Core.Message.ITimelineMessage
        {
            public Guid Id { get; set; }

            public Type ExecutedType { get; set; }

            public MethodInfo ExecutedMethod { get; set; }

            public string EventName { get; set; }

            public Glimpse.Core.Message.TimelineCategory EventCategory { get; set; }

            public string EventSubText { get; set; }

            public TimeSpan Offset { get; set; }

            public TimeSpan Duration { get; set; }

            public DateTime StartTime { get; set; }

            public void BuildDetails(IDictionary<string, object> details)
            {
            }
        }
    }
}
