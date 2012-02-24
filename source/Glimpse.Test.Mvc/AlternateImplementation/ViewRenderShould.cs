using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    public class ViewRenderShould
    {
        [Fact]
        public void Construct()
        {
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();
            var render = new View.Render(brokerMock.Object, ()=>timerMock.Object);

            Assert.NotNull(render);
        }

        [Fact]
        public void ReturnProperType()
        {
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();
            var render = new View.Render(brokerMock.Object, ()=>timerMock.Object);

            Assert.Equal("Render", render.MethodToImplement.Name);
        }

        [Fact]
        public void PublishMessageWithNewImplementation()
        {
            var expectedDuration = TimeSpan.FromMilliseconds(200);
            var expectedOffset = 5;
            var expectedViewName = "a name";
            var expectedIsPartial = false;
            var expectedViewContext = new ViewContext();

            var brokerMock = new Mock<IMessageBroker>();
            brokerMock.Setup(b => b.Publish(It.IsAny<ViewRenderCall>())).Callback<ViewRenderCall>(call =>
                                                                                      {
                                                                                          Assert.Equal(expectedDuration, call.Duration);
                                                                                          Assert.Equal(expectedOffset, call.Offset);
                                                                                          Assert.Equal(expectedIsPartial, call.IsPartial);
                                                                                          Assert.Equal(expectedViewName, call.ViewName);
                                                                                          Assert.Equal(expectedViewContext.ViewData, call.ViewData);
                                                                                          Assert.Equal(expectedViewContext.TempData, call.TempData);
                                                                                      });
            var timerMock = new Mock<IExecutionTimer>();
            timerMock.Setup(t => t.Time(It.IsAny<Action>())).Returns(new FunctionTimerResult{Duration = expectedDuration, Offset = expectedOffset});
            var render = new View.Render(brokerMock.Object, () => timerMock.Object);

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.Proxy).Returns(new ViewMixin{ViewName = expectedViewName, IsPartial = expectedIsPartial});
            contextMock.Setup(c => c.Arguments).Returns(new object[] {expectedViewContext});
            
            
            render.NewImplementation(contextMock.Object);

            timerMock.Verify(t=>t.Time(It.IsAny<Action>()), Times.Once());
            brokerMock.Verify(b=>b.Publish(It.IsAny<ViewRenderCall>()), Times.Once());
        }

        [Fact]
        public void ReturnAllImplementations()
        {
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();
            var imps = View.All(brokerMock.Object, () => timerMock.Object);

            Assert.Equal(1, imps.Count());
        }
    }
}