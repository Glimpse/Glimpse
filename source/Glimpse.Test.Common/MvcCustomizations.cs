using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using Ploeh.AutoFixture;

namespace Glimpse.Test.Common
{
    public class MvcCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            ControllerDescriptor(fixture);
            ActionDescriptor(fixture);
            ActionExecutedContext(fixture);
            ActionExecutingContext(fixture);
            ViewEngineResult(fixture);
            ViewContext(fixture);
            ControllerBase(fixture);
            IValueProvider(fixture);
            ModelBindingContext(fixture);
        }

        private static void ModelBindingContext(IFixture fixture)
        {
            fixture.Customize<ModelBindingContext>(context => context.OmitAutoProperties());
        }

// ReSharper disable InconsistentNaming
        private static void IValueProvider(IFixture fixture)
// ReSharper restore InconsistentNaming
        {
            fixture.Register<IValueProvider>(() =>
                {
                    var mock = new Mock<IValueProvider>();
                    mock.Setup(x => x.GetValue("action")).Returns(new ValueProviderResult("action", null, null));
                    mock.Setup(x => x.GetValue("controller")).Returns(new ValueProviderResult("controller", null, null));
                    return mock.Object;
                });
        }

        private static void ControllerBase(IFixture fixture)
        {
            fixture.Register<IValueProvider, ControllerBase>(
                valueProvider =>
                    {
                        var mock = new Mock<ControllerBase>();
                        mock.Object.ValueProvider = valueProvider;
                        return mock.Object;
                    });
        }

        private static void ViewContext(IFixture fixture)
        {
            fixture.Register<ControllerBase, ViewContext>(
                controllerBase =>
                new ViewContext
                    {TempData = new TempDataDictionary(), ViewData = new ViewDataDictionary(), Controller = controllerBase});
        }

        private static void ViewEngineResult(IFixture fixture)
        {
            fixture.Register<IView, IViewEngine, ViewEngineResult>(
                (view, viewEngine) => new ViewEngineResult(view, viewEngine));
        }

        private static void ActionExecutingContext(IFixture fixture)
        {
            fixture.Register<ControllerContext, ActionDescriptor, ActionExecutingContext>(
                (controllerContext, actionDescriptor) => new ActionExecutingContext(
                                                             controllerContext,
                                                             actionDescriptor,
                                                             fixture.CreateAnonymous<IDictionary<string, object>>())
                                                             {
                                                                 Result = fixture.CreateAnonymous<ActionResult>()
                                                             });
        }

        private static void ActionExecutedContext(IFixture fixture)
        {
            fixture.Register<ControllerContext, ActionDescriptor, bool, Exception, ActionExecutedContext>(
                (controllerContext, actionDescriptor, canceled, exception) =>
                new ActionExecutedContext(controllerContext, actionDescriptor, canceled, exception)
                    {
                        Result = fixture.CreateAnonymous<ActionResult>()
                    });
        }

        private static void ControllerDescriptor(IFixture fixture)
        {
            fixture.Register<string, ControllerDescriptor>(
                (controllerName) =>
                    {
                        var mock = new Mock<ControllerDescriptor>();
                        mock.Setup(m => m.ControllerName).Returns(controllerName);
                        mock.Setup(m => m.ControllerType).Returns(typeof(Controller));
                        return mock.Object;
                    });
        }

        private static void ActionDescriptor(IFixture fixture)
        {
            fixture.Register<string, ControllerDescriptor, ActionDescriptor>(
                (actionName, controllerDescriptor) =>
                    {
                        var mock = new Mock<ActionDescriptor>();
                        mock.Setup(m => m.ActionName).Returns(actionName);
                        mock.Setup(m => m.ControllerDescriptor).Returns(controllerDescriptor);
                        return mock.Object;
                    });
        }
    }
}