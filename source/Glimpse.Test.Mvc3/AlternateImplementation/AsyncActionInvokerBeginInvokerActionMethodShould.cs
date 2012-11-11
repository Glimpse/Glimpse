using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class AsyncActionInvokerBeginInvokerActionMethodShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            IAlternateImplementation<AsyncControllerActionInvoker> sut = new AsyncActionInvoker.BeginInvokeActionMethod();

            Assert.Equal("BeginInvokeActionMethod", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(AsyncActionInvoker.BeginInvokeActionMethod sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.Verify(c => c.Proxy, Times.Never());
        }

        [Theory, AutoMock]
        public void StartTimingExecution(AsyncActionInvoker.BeginInvokeActionMethod sut, IAlternateImplementationContext context, IActionInvokerStateMixin mixin)
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
            mixin.VerifySet(m => m.Offset = It.IsAny<double>());
        }
    }
}