using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Message;
using Glimpse.Test.Mvc3.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
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
            var implementations = ViewEngine.AllMethods();

            Assert.Equal(2, implementations.Count());
        }

        [Fact]
        public void Construct()
        {
            var implementation = new ViewEngine.FindViews(false);

            Assert.NotNull(implementation);
            Assert.NotNull(implementation as IAlternateImplementation<IViewEngine>);
        }

        [Fact]
        public void MethodToImplementIsRight()
        {
            Assert.Equal(typeof(IViewEngine), Implementation.MethodToImplement.DeclaringType);
        }

        [Fact]
        public void ProceedIfRuntimePolicyIsOff()
        {
            var findViews = new ViewEngine.FindViews(false);
            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(Implementation.MessageBrokerMock.Object);
            contextMock.Setup(c => c.ProxyFactory).Returns(Implementation.ProxyFactoryMock.Object);
            contextMock.Setup(c => c.Logger).Returns(Implementation.LoggerMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(() => new ExecutionTimer(Stopwatch.StartNew()));
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            findViews.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
        }

        [Fact]
        public void PublishMessagesIfRuntimePolicyIsOnAndViewNotFound()
        {
            var findViews = new ViewEngine.FindViews(false);

            var controllerContext = new ControllerContext();
            var viewName = "anything";
            var useCache = true;

            var args = new object[] { controllerContext, viewName, "MasterName", useCache };

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.Arguments).Returns(args);
            contextMock.Setup(c => c.ReturnValue).Returns(new ViewEngineResult(Enumerable.Empty<string>()));
            contextMock.Setup(c => c.MessageBroker).Returns(Implementation.MessageBrokerMock.Object);
            contextMock.Setup(c => c.ProxyFactory).Returns(Implementation.ProxyFactoryMock.Object);
            contextMock.Setup(c => c.Logger).Returns(Implementation.LoggerMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(() => new ExecutionTimer(Stopwatch.StartNew()));
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.On);

            findViews.NewImplementation(contextMock.Object);

            Implementation.MessageBrokerMock.Verify(b=>b.Publish(It.IsAny<ViewEngine.FindViews.Message>()));
            Implementation.MessageBrokerMock.Verify(b=>b.Publish(It.IsAny<TimerResultMessage>()));
        }

        [Fact]
        public void PublishMessagesIfRuntimePolicyIsOnAndViewIsFound()
        {
            var viewMock = new Mock<IView>();
            var engineMock = new Mock<IViewEngine>();

            Implementation.ProxyFactoryMock.Setup(p => p.IsProxyable(It.IsAny<object>())).Returns(true);
            Implementation.ProxyFactoryMock.Setup(
                p =>
                p.CreateProxy(It.IsAny<IView>(), It.IsAny<IEnumerable<IAlternateImplementation<IView>>>(),
                              It.IsAny<object>())).Returns(viewMock.Object);

            var findViews = new ViewEngine.FindViews(false);

            var controllerContext = new ControllerContext();
            var viewName = "anything";
            var useCache = true;

            var args = new object[] { controllerContext, viewName, "MasterName", useCache };

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.Arguments).Returns(args);
            contextMock.Setup(c => c.MessageBroker).Returns(Implementation.MessageBrokerMock.Object);
            contextMock.Setup(c => c.ProxyFactory).Returns(Implementation.ProxyFactoryMock.Object);
            contextMock.Setup(c => c.Logger).Returns(Implementation.LoggerMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(() => new ExecutionTimer(Stopwatch.StartNew()));
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.On);

            var viewEngineResult = new ViewEngineResult(viewMock.Object, engineMock.Object);
            contextMock.Setup(c => c.ReturnValue).Returns(viewEngineResult);

            findViews.NewImplementation(contextMock.Object);

            Implementation.ProxyFactoryMock.Verify(p=>p.IsProxyable(It.IsAny<object>()));
            Implementation.LoggerMock.Verify(l=>l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
            contextMock.VerifySet(c=>c.ReturnValue = It.IsAny<ViewEngineResult>());
            Implementation.MessageBrokerMock.Verify(b => b.Publish(It.IsAny<ViewEngine.FindViews.Message>()));
            Implementation.MessageBrokerMock.Verify(b => b.Publish(It.IsAny<TimerResultMessage>()));
        }
    }
}