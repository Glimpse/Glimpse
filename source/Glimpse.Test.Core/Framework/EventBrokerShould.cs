using System;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class EventBrokerShould
    {
        [Fact]
        public void Construct()
        {
            var loggerMock = new Mock<ILogger>();

            var eventBroker = new MessageBroker(loggerMock.Object);

            Assert.NotNull(eventBroker);
        }

        [Fact]
        public void SubscribeToEvents()
        {
            var loggerMock = new Mock<ILogger>();
            var eventBroker = new MessageBroker(loggerMock.Object);

            eventBroker.Subscribe<DummyMessage>(evt => Assert.IsType<DummyMessage>(evt));

            Assert.True(eventBroker.Subscriptions[typeof(DummyMessage)].Any());
        }

        [Fact]
        public void SubscribeToEventsGrowsSubscriberbase()
        {
            var loggerMock = new Mock<ILogger>();
            var eventBroker = new MessageBroker(loggerMock.Object);

            eventBroker.Subscribe<DummyMessage>(evt => Assert.IsType<DummyMessage>(evt));

            Assert.True(eventBroker.Subscriptions[typeof(DummyMessage)].Count() == 1);

            eventBroker.Subscribe<DummyMessage>(Assert.NotNull);

            Assert.True(eventBroker.Subscriptions[typeof(DummyMessage)].Count() == 2);
        }

        [Fact]
        public void UnsubscribeFromEvent()
        {
            var loggerMock = new Mock<ILogger>();
            var eventBroker = new MessageBroker(loggerMock.Object);

            var subId = eventBroker.Subscribe<DummyMessage>(evt => Assert.IsType<DummyMessage>(evt));

            Assert.True(eventBroker.Subscriptions[typeof(DummyMessage)].Any());

            eventBroker.Unsubscribe<DummyMessage>(subId);

            Assert.False(eventBroker.Subscriptions[typeof(DummyMessage)].Any());
        }

        [Fact]
        public void Publish()
        {
            var counter = 0;
            var expected = "Any Value";
            var message = new DummyMessage {Id = expected};

            var loggerMock = new Mock<ILogger>();
            var eventBroker = new MessageBroker(loggerMock.Object);
            var subId = eventBroker.Subscribe<DummyMessage>(evt =>
                                                                {
                                                                    Assert.Equal(expected, evt.Id);
                                                                    counter++;
                                                                });

            eventBroker.Publish(message);

            Assert.Equal(1, counter);
        }

        [Fact]
        public void LogSubscriptions()
        {
            var loggerMock = new Mock<ILogger>();

            var eventBroker = new MessageBroker(loggerMock.Object);

            eventBroker.Subscribe<DummyMessage>(Assert.NotNull);

            loggerMock.Verify(l=>l.Debug(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Fact]
        public void ThrowWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(()=>new MessageBroker(null));
        }

        [Fact]
        public void HandleExceptionsFromSubscribers()
        {
            var loggerMock = new Mock<ILogger>();

            var eventBroker = new MessageBroker(loggerMock.Object);

            eventBroker.Subscribe<DummyMessage>(m => { throw new DummyException(); });
            eventBroker.Publish(new DummyMessage());

            loggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
        }

        [Fact]
        public void ThrowWithNullSubscriptionAction()
        {
            var loggerMock = new Mock<ILogger>();

            var eventBroker = new MessageBroker(loggerMock.Object);

            Assert.Throws<ArgumentNullException>(()=>eventBroker.Subscribe<DummyMessage>(null));
        }
    }
}