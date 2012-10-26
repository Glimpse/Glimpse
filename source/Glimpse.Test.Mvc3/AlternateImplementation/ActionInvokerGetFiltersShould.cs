using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class ControllerActionInvokerGetFiltersShould : ActionInvokerGetFiltersShould<ControllerActionInvoker>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class AsyncControllerActionInvokerGetFiltersShould : ActionInvokerGetFiltersShould<AsyncControllerActionInvoker>
    {
    }

    public abstract class ActionInvokerGetFiltersShould<T> where T : class
    {
        [Fact]
        public void ImplementProperMethod()
        {
            var implementation = new ActionInvoker.GetFilters<T>();

            Assert.Equal("GetFilters", implementation.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            var implementation = new ActionInvoker.GetFilters<T>();

            implementation.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.Verify(c => c.ReturnValue, Times.Never());
        }

        [Theory, AutoMock]
        public void ProxyFiltersWithRuntimePolicyOn([Frozen] IExecutionTimer timer, IAlternateImplementationContext context)
        {
            var implementation = new ActionInvoker.GetFilters<T>();

            implementation.NewImplementation(context);

            timer.Verify(t => t.Time(It.IsAny<Action>()));
            context.Verify(c => c.ReturnValue);
        }
    }
}