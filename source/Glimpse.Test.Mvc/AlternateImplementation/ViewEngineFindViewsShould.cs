using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Message;
using Glimpse.Test.Mvc.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc.AlternateImplementation
{
    public class ViewEngineFindViewsShould
    {
        [Fact]
        public void ReturnAllMethodImplementationsWithStaticAll()
        {
            var brokerMock = new Mock<IMessageBroker>();
            var factoryMock = new Mock<IProxyFactory>();
            var timerMock = new Mock<IExecutionTimer>();

            var alternateImplementations = ViewEngine.All(brokerMock.Object, factoryMock.Object, () => timerMock.Object);

            Assert.Equal(2, alternateImplementations.Count());
        }

        [Fact]
        public void Construct()
        {
            var factoryMock = new Mock<IProxyFactory>();
            var brokerMock = new Mock<IMessageBroker>();
            var findView = new ViewEngine.FindViews(brokerMock.Object, factoryMock.Object, ()=>new ExecutionTimer(Stopwatch.StartNew()), false);

            Assert.NotNull(findView);
            Assert.NotNull(findView as IAlternateImplementation<IViewEngine>);
        }

        [Fact]
        public void MethodToImplementIsRight()
        {
            var factoryMock = new Mock<IProxyFactory>();
            var brokerMock = new Mock<IMessageBroker>();
            var findView = new ViewEngine.FindViews(brokerMock.Object, factoryMock.Object, () => new ExecutionTimer(Stopwatch.StartNew()), false);

            var method = findView.MethodToImplement;

            Assert.Equal("FindView", method.Name);
            Assert.Equal(typeof(IViewEngine), method.DeclaringType);
        }

        [Fact]
        public void LeaveDefaultViewResultWhenViewNotFound()
        {
            var expectedViewEngineResult = new ViewEngineResult(new[] {"one", "two"});
            var expectedDuration = TimeSpan.FromMilliseconds(500);
            var expectedOffset = 5;
            var expectedTagetType = typeof (ViewEngineFindViewsShould);
            var expectedViewName = "viewName";
            var expectedMasterName = "masterName";

            var timerMock = new Mock<IExecutionTimer>();
            timerMock.Setup(t => t.Time(It.IsAny<Action>())).Returns(new FunctionTimerResult
                                                                         {
                                                                             Duration = expectedDuration,
                                                                             Offset = expectedOffset
                                                                         });
            var brokerMock = new Mock<IMessageBroker>();
            brokerMock.Setup(b => b.Publish(It.IsAny<ViewEngineFindCall>())).Callback<ViewEngineFindCall>(call =>
                                                                                                      {
                                                                                                          Assert.Equal(expectedDuration, call.Duration);
                                                                                                          Assert.Equal(expectedOffset, call.Offset);
                                                                                                          Assert.Equal(expectedViewEngineResult, call.ViewEngineResult);
                                                                                                          Assert.Equal(expectedTagetType, call.ViewEngineType);
                                                                                                          Assert.Equal(expectedViewName, call.ViewName);
                                                                                                          Assert.Equal(expectedMasterName, call.MasterName);
                                                                                                          Assert.False(call.IsPartial);
                                                                                                      });
            var factoryMock = new Mock<IProxyFactory>();
            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.ReturnValue).Returns(expectedViewEngineResult);
            contextMock.Setup(c => c.TargetType).Returns(expectedTagetType);
            contextMock.Setup(c => c.Arguments).Returns(new object[]{null, expectedViewName, expectedMasterName, true});

            var findView = new ViewEngine.FindViews(brokerMock.Object, factoryMock.Object, () => timerMock.Object, false);

            findView.NewImplementation(contextMock.Object);

            timerMock.Verify(t=>t.Time(It.IsAny<Action>()), Times.Once());
            contextMock.VerifyGet(c=>c.ReturnValue, Times.Once());
            contextMock.VerifySet(c=>c.ReturnValue = It.IsAny<object>(), Times.Never());
            brokerMock.Verify(b=>b.Publish(It.IsAny<ViewEngineFindCall>()));
        }

        [Fact]
        public void ProxyViewResultWhenViewIsFound()
        {
            var expectedViewEngineResult = new ViewEngineResult(new DummyView(), new Mock<IViewEngine>().Object);
            var expectedDuration = TimeSpan.FromMilliseconds(500);
            var expectedOffset = 5;
            var expectedTagetType = typeof(ViewEngineFindViewsShould);
            var expectedViewName = "viewName";
            var expectedMasterName = "masterName";

            var timerMock = new Mock<IExecutionTimer>();
            timerMock.Setup(t => t.Time(It.IsAny<Action>())).Returns(new FunctionTimerResult
            {
                Duration = expectedDuration,
                Offset = expectedOffset
            });
            var brokerMock = new Mock<IMessageBroker>();
            brokerMock.Setup(b => b.Publish(It.IsAny<ViewEngineFindCall>())).Callback<ViewEngineFindCall>(call =>
            {
                Assert.Equal(expectedDuration, call.Duration);
                Assert.Equal(expectedOffset, call.Offset);
                Assert.Equal(expectedViewEngineResult, call.ViewEngineResult);
                Assert.Equal(expectedTagetType, call.ViewEngineType);
                Assert.Equal(expectedViewName, call.ViewName);
                Assert.Equal(expectedMasterName, call.MasterName);
                Assert.False(call.IsPartial);
            });

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.ReturnValue).Returns(expectedViewEngineResult);
            contextMock.Setup(c => c.TargetType).Returns(expectedTagetType);
            contextMock.Setup(c => c.Arguments).Returns(new object[] { null, expectedViewName, expectedMasterName, true });
            

            var findView = new ViewEngine.FindViews(brokerMock.Object, new CastleDynamicProxyFactory(new Mock<ILogger>().Object), () => timerMock.Object, false);

            findView.NewImplementation(contextMock.Object);

            timerMock.Verify(t => t.Time(It.IsAny<Action>()), Times.Once());
            contextMock.VerifyGet(c => c.ReturnValue, Times.AtLeastOnce());
            contextMock.VerifySet(c => c.ReturnValue = It.IsAny<ViewEngineResult>(), Times.Once());
            brokerMock.Verify(b => b.Publish(It.IsAny<ViewEngineFindCall>()));
        }

        [Fact]
        public void LeaveDefaultPartialViewResultWhenPartialViewNotFound()
        {
            var expectedViewEngineResult = new ViewEngineResult(new[] { "one", "two" });
            var expectedDuration = TimeSpan.FromMilliseconds(500);
            var expectedOffset = 5;
            var expectedTagetType = typeof(ViewEngineFindViewsShould);
            var expectedViewName = "viewName";

            var timerMock = new Mock<IExecutionTimer>();
            timerMock.Setup(t => t.Time(It.IsAny<Action>())).Returns(new FunctionTimerResult
            {
                Duration = expectedDuration,
                Offset = expectedOffset
            });
            var brokerMock = new Mock<IMessageBroker>();
            brokerMock.Setup(b => b.Publish(It.IsAny<ViewEngineFindCall>())).Callback<ViewEngineFindCall>(call =>
            {
                Assert.Equal(expectedDuration, call.Duration);
                Assert.Equal(expectedOffset, call.Offset);
                Assert.Equal(expectedViewEngineResult, call.ViewEngineResult);
                Assert.Equal(expectedTagetType, call.ViewEngineType);
                Assert.Equal(expectedViewName, call.ViewName);
                Assert.True(call.IsPartial);
            });
            var factoryMock = new Mock<IProxyFactory>();
            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.ReturnValue).Returns(expectedViewEngineResult);
            contextMock.Setup(c => c.TargetType).Returns(expectedTagetType);
            contextMock.Setup(c => c.Arguments).Returns(new object[] { null, expectedViewName, true });

            var findView = new ViewEngine.FindViews(brokerMock.Object, factoryMock.Object, () => timerMock.Object, true);

            findView.NewImplementation(contextMock.Object);

            timerMock.Verify(t => t.Time(It.IsAny<Action>()), Times.Once());
            contextMock.VerifyGet(c => c.ReturnValue, Times.Once());
            contextMock.VerifySet(c => c.ReturnValue = It.IsAny<object>(), Times.Never());
            brokerMock.Verify(b => b.Publish(It.IsAny<ViewEngineFindCall>()));
        }

        [Fact]
        public void ProxyPartialViewResultWhenPartialViewIsFound()
        {
            var expectedViewEngineResult = new ViewEngineResult(new DummyView(), new Mock<IViewEngine>().Object);
            var expectedDuration = TimeSpan.FromMilliseconds(500);
            var expectedOffset = 5;
            var expectedTagetType = typeof(ViewEngineFindViewsShould);
            var expectedViewName = "viewName";

            var timerMock = new Mock<IExecutionTimer>();
            timerMock.Setup(t => t.Time(It.IsAny<Action>())).Returns(new FunctionTimerResult
            {
                Duration = expectedDuration,
                Offset = expectedOffset
            });
            var brokerMock = new Mock<IMessageBroker>();
            brokerMock.Setup(b => b.Publish(It.IsAny<ViewEngineFindCall>())).Callback<ViewEngineFindCall>(call =>
            {
                Assert.Equal(expectedDuration, call.Duration);
                Assert.Equal(expectedOffset, call.Offset);
                Assert.Equal(expectedViewEngineResult, call.ViewEngineResult);
                Assert.Equal(expectedTagetType, call.ViewEngineType);
                Assert.Equal(expectedViewName, call.ViewName);
                Assert.True(call.IsPartial);
            });

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.ReturnValue).Returns(expectedViewEngineResult);
            contextMock.Setup(c => c.TargetType).Returns(expectedTagetType);
            contextMock.Setup(c => c.Arguments).Returns(new object[] { null, expectedViewName, true });


            var findView = new ViewEngine.FindViews(brokerMock.Object, new CastleDynamicProxyFactory(new Mock<ILogger>().Object), () => timerMock.Object, true);

            findView.NewImplementation(contextMock.Object);

            timerMock.Verify(t => t.Time(It.IsAny<Action>()), Times.Once());
            contextMock.VerifyGet(c => c.ReturnValue, Times.AtLeastOnce());
            contextMock.VerifySet(c => c.ReturnValue = It.IsAny<ViewEngineResult>(), Times.Once());
            brokerMock.Verify(b => b.Publish(It.IsAny<ViewEngineFindCall>()));
        }
    }
}