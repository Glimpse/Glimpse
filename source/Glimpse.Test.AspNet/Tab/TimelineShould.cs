using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Tab;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
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

            Assert.Equal(typeof(HttpContextBase), timeline.RequestContextType);
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
            context.TabStore.Setup(x => x.Contains(typeof(IList<ITimerResultMessage>).AssemblyQualifiedName)).Returns(true);
            context.TabStore.Setup(x => x.Get(typeof(IList<ITimerResultMessage>).AssemblyQualifiedName)).Returns(BuildMessages());

            var timeline = new Timeline();
            var result = timeline.GetData(context) as TimelineModel;

            Assert.NotNull(result);
            Assert.Equal(7, result.Duration);
            Assert.Equal(3, result.Events.Count);
            Assert.Equal("TestName1", result.Events[0].Title);
            Assert.Equal("TestName3", result.Events[1].Title);
            Assert.Equal("TestName2", result.Events[2].Title);
        }

        [Theory, AutoMock]
        public void ReturnEmptyWhenNoData(ITabContext context)
        {
            context.TabStore.Setup(x => x.Get(typeof(ITimerResultMessage).FullName)).Returns((IEnumerable<ITimerResultMessage>)null);

            var timeline = new Timeline();
            var result = timeline.GetData(context) as TimelineModel;

            Assert.NotNull(result);
            Assert.Equal(0, result.Duration);
            Assert.Null(result.Events);
        } 

        private IEnumerable<ITimerResultMessage> BuildMessages()
        {
            return new List<ITimerResultMessage>
                               {
                                   new TestTimerResultMessage { Duration = 1, EventCategory = "Test1", EventName = "TestName1", EventSubText = "TestSub1", Offset = 1, StartTime = DateTime.Now },
                                   new TestTimerResultMessage { Duration = 4, EventCategory = "Test2", EventName = "TestName2", EventSubText = "TestSub2", Offset = 3, StartTime = DateTime.Now },
                                   new TestTimerResultMessage { Duration = 1, EventCategory = "Test3", EventName = "TestName3", EventSubText = "TestSub3", Offset = 2, StartTime = DateTime.Now }
                               };
        }

        public class TestTimerResultMessage : ITimerResultMessage
        {
            public Guid Id { get; set; }

            public string EventName { get; set; }

            public string EventCategory { get; set; }

            public string EventSubText { get; set; }

            public double Offset { get; set; }

            public double Duration { get; set; }

            public DateTime StartTime { get; set; }

            public void BuildDetails(IDictionary<string, object> details)
            {
            }
        }
    }
}
