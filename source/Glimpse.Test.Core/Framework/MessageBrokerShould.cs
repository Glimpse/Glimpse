using System;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Test.Common;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.Framework
{
    public class MessageBrokerShould
    {
        [Theory, AutoMock]
        public void Construct(ILogger logger)
        {
            var sut = new MessageBroker(() => true, logger);

            Assert.NotNull(sut);
            Assert.Equal(logger, sut.Logger);
        }

        [Theory, AutoMock]
        public void SubscribeToEvents(MessageBroker sut)
        {
            sut.Subscribe<DummyMessage>(evt => Assert.IsType<DummyMessage>(evt));

            Assert.True(sut.Subscriptions[typeof(DummyMessage)].Any());
        }

        [Theory, AutoMock]
        public void SubscribeToEventsGrowsSubscriberbase(MessageBroker sut)
        {
            sut.Subscribe<DummyMessage>(evt => Assert.IsType<DummyMessage>(evt));

            Assert.True(sut.Subscriptions[typeof(DummyMessage)].Count() == 1);

            sut.Subscribe<DummyMessage>(Assert.NotNull);

            Assert.True(sut.Subscriptions[typeof(DummyMessage)].Count() == 2);
        }

        [Theory, AutoMock]
        public void UnsubscribeFromEvent(MessageBroker sut)
        {
            var subId = sut.Subscribe<DummyMessage>(evt => Assert.IsType<DummyMessage>(evt));

            Assert.True(sut.Subscriptions[typeof(DummyMessage)].Any());

            sut.Unsubscribe<DummyMessage>(subId);

            Assert.False(sut.Subscriptions[typeof(DummyMessage)].Any());
        }

        [Theory, AutoMock]
        public void Publish(MessageBroker sut, string expected)
        {
            var counter = 0;
            var message = new DummyMessage { Identifier = expected };

            sut.Subscribe<DummyMessage>(
                evt =>
                {
                    Assert.Equal(expected, evt.Identifier);
                    counter++;
                });

            sut.Publish(message);

            Assert.Equal(1, counter);
        }

        [Theory, AutoMock]
        public void NotPublishWhenIndicated()
        {
            var sut = new MessageBroker(() => false, new NullLogger());

            var counter = 0;
            sut.Subscribe<DummyMessage>(_ => counter++);

            sut.Publish(new DummyMessage());

            Assert.Equal(0, counter);
        }

        [Theory, AutoMock]
        public void LogSubscriptions(MessageBroker sut)
        {
            sut.Subscribe<DummyMessage>(Assert.NotNull);

            sut.Logger.Verify(l => l.Debug(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Fact]
        public void ThrowWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new MessageBroker(() => true, null));
        }

        [Theory, AutoMock]
        public void HandleExceptionsFromSubscribers(MessageBroker sut)
        {
            sut.Subscribe<DummyMessage>(m => { throw new DummyException(); });
            sut.Publish(new DummyMessage());

            sut.Logger.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
        }

        [Theory, AutoMock]
        public void ThrowWithNullSubscriptionAction(MessageBroker sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Subscribe<DummyMessage>(null));
        }

        [Theory, AutoMock]
        public void HandleInterfaceBasedSubscriptions(MessageBroker sut)
        {
            var counter = 0;

            sut.Subscribe<IDummyInterface>(m => counter++);

            sut.Publish(new DummyMessage());

            Assert.Equal(1, counter);
        }

        [Theory, AutoMock]
        public void HandleMessageInheritanceChains(MessageBroker sut)
        {
            var counter = 0;

            sut.Subscribe<MessageBase>(m => counter++);

            sut.Publish(new DummyMessage());

            Assert.Equal(1, counter);
        }

        [Theory, AutoMock]
        public void IgnoreUnknownTypes(MessageBroker sut)
        {
            var counter = 0;

            sut.Subscribe<IDisposable>(m => counter++);

            sut.Publish(new DummyMessage());

            Assert.Equal(0, counter);
        }
    }
}