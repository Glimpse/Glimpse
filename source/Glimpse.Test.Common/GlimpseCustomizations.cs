using System.ComponentModel;
using System.IO;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Ploeh.AutoFixture;

namespace Glimpse.Test.Common
{
    public class GlimpseCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            IAlternateImplementationContext(fixture);

            ViewEngineFindViewArguments(fixture);

            ViewRenderArguments(fixture);

            ModelBindBindModelArguments(fixture);

            ModelBinderBindPropertyArguments(fixture);

            ValueProviderGetValueArguments(fixture);

            ControllerFactoryCreateController(fixture);
        }

        private static void ControllerFactoryCreateController(IFixture fixture)
        {
            fixture.Register<IProxyFactory, ControllerFactory.CreateController>(
                (proxyFactory) =>
                new ControllerFactory.CreateController(new ActionInvoker(proxyFactory), new AsyncActionInvoker(proxyFactory)));
        }

        private static void ValueProviderGetValueArguments(IFixture fixture)
        {
            fixture.Register<string, ValueProvider<IValueProvider>.GetValue.Arguments>(
                key =>
                new ValueProvider<IValueProvider>.GetValue.Arguments(key));
            fixture.Register<string, bool, ValueProvider<IUnvalidatedValueProvider>.GetValue.Arguments>(
                (key, skipValidation) =>
                new ValueProvider<IUnvalidatedValueProvider>.GetValue.Arguments(key, skipValidation));
        }

        private static void ModelBinderBindPropertyArguments(IFixture fixture)
        {
            fixture.Register<ControllerContext, ModelBindingContext, PropertyDescriptor, ModelBinder.BindProperty.Arguments>(
                (controllerContext, modelBindingContext, propertyDescriptor) =>
                new ModelBinder.BindProperty.Arguments(controllerContext, modelBindingContext, propertyDescriptor));
        }

        private static void ModelBindBindModelArguments(IFixture fixture)
        {
            fixture.Register<ControllerContext, ModelBindingContext, ModelBinder.BindModel.Arguments>(
                (controllerContext, modelBindingContext) =>
                new ModelBinder.BindModel.Arguments(controllerContext, modelBindingContext));
        }

        private static void ViewRenderArguments(IFixture fixture)
        {
            fixture.Register<ViewContext, View.Render.Arguments>(
                viewContext =>
                new View.Render.Arguments(viewContext, new StringWriter()));
        }

        private static void ViewEngineFindViewArguments(IFixture fixture)
        {
            fixture.Register<string, bool, ViewEngine.FindViews.Arguments>(
                (viewName, isPartial) =>
                new ViewEngine.FindViews.Arguments(isPartial, new ControllerContext(), viewName, true));
        }

// ReSharper disable InconsistentNaming
        private static void IAlternateImplementationContext(IFixture fixture)
// ReSharper restore InconsistentNaming
        {
            fixture.Register<IMessageBroker, IProxyFactory, IExecutionTimer, ILogger, IAlternateImplementationContext>(
                (broker, proxy, timer, logger) =>
                    {
                        var mock = new Mock<IAlternateImplementationContext>();
                        mock.Setup(m => m.MessageBroker).Returns(broker);
                        mock.Setup(m => m.ProxyFactory).Returns(proxy);
                        mock.Setup(m => m.RuntimePolicyStrategy).Returns(() => RuntimePolicy.On);
                        mock.Setup(m => m.TimerStrategy).Returns(() => timer);
                        mock.Setup(m => m.InvocationTarget).Returns(new object());
                        mock.Setup(m => m.MethodInvocationTarget).Returns(typeof(object).GetMethod("ToString"));
                        mock.Setup(m => m.Logger).Returns(logger);
                        return mock.Object;
                    });
        }
    }
}