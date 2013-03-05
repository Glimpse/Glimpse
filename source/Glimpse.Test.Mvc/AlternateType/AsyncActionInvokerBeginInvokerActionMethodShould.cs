using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.AlternateType
{
    public class AsyncActionInvokerBeginInvokerActionMethodShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            IAlternateMethod sut = new AsyncActionInvoker.BeginInvokeActionMethod();

            Assert.Equal("BeginInvokeActionMethod", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(AsyncActionInvoker.BeginInvokeActionMethod sut, IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.Verify(c => c.Proxy, Times.Never());
        }

        [Theory, AutoMock]
        public void StartTimingExecution(AsyncActionInvoker.BeginInvokeActionMethod sut, IAlternateMethodContext context, IActionInvokerStateMixin mixin)
        {
            context.Setup(c => c.Proxy).Returns(mixin);
            context.Setup(c => c.Arguments).Returns(new object[]
                                                            {
                                                                new ControllerContext(),
                                                                new Mock<ActionDescriptor>().Object,
                                                                new Dictionary<string, object>(),
                                                                new AsyncCallback(delegate { }),
                                                                "state"
                                                            });

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.Verify(c => c.Proxy);
            mixin.VerifySet(m => m.Offset = It.IsAny<TimeSpan>());
        }
    }
}