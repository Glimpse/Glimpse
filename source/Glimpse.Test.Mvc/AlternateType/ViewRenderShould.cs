using System;
using System.IO;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.AlternateType
{
    public class ViewRenderShould
    {
        [Fact]
        public void SetProperties()
        {
            var sut = new View.Render();

            Assert.NotNull(sut.MethodToImplement);
        }
        
        [Theory, AutoMock]
        public void PublishMessagesWithOnPolicy(View.Render sut, IAlternateMethodContext context, IViewCorrelationMixin mixin, ViewContext viewContext)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { viewContext, new StringWriter() });
            context.Setup(c => c.Proxy).Returns(mixin);

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<View.Render.Message>())); 
        } 

        [Theory, AutoMock]
        public void ProceedWithOffPolicy(View.Render sut, IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()), Times.Never());
        } 
    }
}