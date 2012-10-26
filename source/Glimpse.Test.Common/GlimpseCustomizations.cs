using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Moq;
using Ploeh.AutoFixture;

namespace Glimpse.Test.Common
{
    public class GlimpseCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            IAlternateImplementationContext(fixture);
        }

        private static void IAlternateImplementationContext(IFixture fixture)
        {
            fixture.Register<IMessageBroker, IProxyFactory, IExecutionTimer, IAlternateImplementationContext>(
                (broker, proxy, timer) =>
                    {
                        var mock = new Mock<IAlternateImplementationContext>();
                        mock.Setup(m => m.MessageBroker).Returns(broker);
                        mock.Setup(m => m.ProxyFactory).Returns(proxy);
                        mock.Setup(m => m.RuntimePolicyStrategy).Returns(() => RuntimePolicy.On);
                        mock.Setup(m => m.TimerStrategy).Returns(() => timer);
                        return mock.Object;
                    });
        }
    }
}