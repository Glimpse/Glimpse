using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc3.TestDoubles;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class ControllerActionInvokerInvokeActionResultShould : ActionInvokerInvokeActionResultShould<ControllerActionInvoker>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class AsyncControllerActionInvokerInvokeActionResultShould : ActionInvokerInvokeActionResultShould<AsyncControllerActionInvoker>
    {
    }

    public abstract class ActionInvokerInvokeActionResultShould<T> where T : class
    {
        [Fact]
        public void ImplementProperMethod()
        {
            var sut = new ActionInvoker.InvokeActionResult<T>();

            Assert.Equal("InvokeActionResult", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(ActionInvoker.InvokeActionResult<T> sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ActionInvoker.InvokeActionResult<T>.Message>()), Times.Never());
        }

        [Theory, AutoMock]
        public void PublishMessageWithRuntimePolicyOn(ActionInvoker.InvokeActionResult<T> sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.Arguments).Returns(new object[]
                                                            {
                                                                new ControllerContext
                                                                    {
                                                                        Controller = new DummyAsyncController(),
                                                                    },
                                                                    new ContentResult()
                                                            });

            sut.NewImplementation(context);

            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ActionInvoker.InvokeActionResult<T>.Message>()));
        }
    }
}