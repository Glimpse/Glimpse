using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class AsyncActionInvokerEndInvokeActionMethodShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            var sut = new AsyncActionInvoker.EndInvokeActionMethod();

            Assert.Equal("EndInvokeActionMethod", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedThenReturnWithRuntimePolicyOff(AsyncActionInvoker.EndInvokeActionMethod sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.Verify(c => c.Proxy, Times.Never());
        }

        [Theory, AutoMock]
        public void PublishMessageWithRuntimePolicyOn(AsyncActionInvoker.EndInvokeActionMethod sut, IAlternateImplementationContext context, ActionDescriptor actionDescriptor)
        {
            context.Setup(c => c.ReturnValue).Returns(new ContentResult());
            context.Setup(c => c.Proxy).Returns(
                new ActionInvokerStateMixin
                {
                    Offset = 10, 
                    Arguments = new ActionInvoker.InvokeActionMethod.Arguments(
                        new object[]
                        {
                            new ControllerContext(),
                            actionDescriptor,
                            new Dictionary<string, object>()
                        })
                });

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ActionInvoker.InvokeActionMethod.Message>()));
        }
    }
}