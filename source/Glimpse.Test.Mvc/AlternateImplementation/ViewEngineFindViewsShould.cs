using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Mvc.TestDoubles;
using Moq;
using Xunit;
using TimerResult = Glimpse.Core2.Extensibility.TimerResult;

namespace Glimpse.Test.Mvc.AlternateImplementation
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

    public class ViewEngineFindViewsShould:IDisposable
    {
        private ViewEngineFindViewsTester implementation;
        public ViewEngineFindViewsTester Implementation
        {
            get { return implementation ?? (implementation = ViewEngineFindViewsTester.Create()); }
            set { implementation = value; }
        }

        public void Dispose()
        {
            Implementation = null;
        }

        [Fact]
        public void ReturnAllMethodImplementationsWithStaticAll()
        {
            var implementations = ViewEngine.AllMethods(Implementation.MessageBrokerMock.Object, Implementation.ProxyFactoryMock.Object, Implementation.LoggerMock.Object, () => Implementation.ExecutionTimerMock.Object, ()=>RuntimePolicy.On);

            Assert.Equal(2, implementations.Count());
        }

        [Fact]
        public void Construct()
        {
            var implementation = new ViewEngine.FindViews(Implementation.MessageBrokerMock.Object, Implementation.ProxyFactoryMock.Object, Implementation.LoggerMock.Object, () => new ExecutionTimer(Stopwatch.StartNew()), ()=>RuntimePolicy.On, false);

            Assert.NotNull(implementation);
            Assert.NotNull(implementation as IAlternateImplementation<IViewEngine>);
        }

        [Fact]
        public void MethodToImplementIsRight()
        {
            Assert.Equal(typeof(IViewEngine), Implementation.MethodToImplement.DeclaringType);
        }
    }
}