using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Mvc.AlternateImplementation;
using Moq;

namespace Glimpse.Test.Mvc.Tester
{
    public class ViewEngineFindViewsTester:ViewEngine.FindViews
    {
        public Mock<IMessageBroker> MessageBrokerMock { get; set; }
        public Mock<IProxyFactory> ProxyFactoryMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }
        public Mock<IExecutionTimer> ExecutionTimerMock { get; set; }

        private ViewEngineFindViewsTester(Mock<IMessageBroker> brokerMock, Mock<IProxyFactory> factoryMock, Mock<ILogger> loggerMock, Mock<IExecutionTimer> timerMock):base(brokerMock.Object, factoryMock.Object, loggerMock.Object, ()=>timerMock.Object,()=>RuntimePolicy.On,  false)
        {
            MessageBrokerMock = brokerMock;
            ProxyFactoryMock = factoryMock;
            LoggerMock = loggerMock;
            ExecutionTimerMock = timerMock;
        }

        public static ViewEngineFindViewsTester Create()
        {
            return new ViewEngineFindViewsTester(new Mock<IMessageBroker>(), new Mock<IProxyFactory>(), new Mock<ILogger>(), new Mock<IExecutionTimer>());
        }
    }
}