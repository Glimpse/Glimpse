using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Inspector;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.Inspector
{
    public class ExecutionShould : IDisposable
    {
        private readonly IControllerFactory controllerFactory;

        public ExecutionShould()
        {
            controllerFactory = ControllerBuilder.Current.GetControllerFactory();
        }

        [Theory, AutoMock]
        public void ContinueIfUnableToProxyControllerFactory(ExecutionInspector sut, IInspectorContext context)
        {
            context.ProxyFactory.Setup(f => f.IsWrapClassEligible(It.IsAny<Type>())).Returns(false);

            sut.Setup(context);

            Assert.IsType<DefaultControllerFactory>(ControllerBuilder.Current.GetControllerFactory());
        }

        [Theory, AutoMock]
        public void ProxyControllerFactory(ExecutionInspector sut, IInspectorContext context, IControllerFactory controllerFactory)
        {
            context.ProxyFactory.Setup(f => f.IsWrapInterfaceEligible<IControllerFactory>(It.IsAny<Type>())).Returns(true);
            context.ProxyFactory.Setup(f => f.WrapInterface(It.IsAny<IControllerFactory>(), It.IsAny<IEnumerable<IAlternateMethod>>(), Enumerable.Empty<object>())).Returns(controllerFactory);

            sut.Setup(context);

            Assert.Equal(ControllerBuilder.Current.GetControllerFactory(), controllerFactory);
            context.Logger.Verify(l => l.Debug(It.Is<string>(s => s.Contains("IControllerFactory")), It.IsAny<object[]>()));
        }

        public void Dispose()
        {
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}