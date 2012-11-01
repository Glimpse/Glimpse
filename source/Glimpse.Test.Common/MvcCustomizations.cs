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
            ResultExecutedContext(fixture);

            ResultExecutingContext(fixture);
        }

        private static void ResultExecutingContext(IFixture fixture)
        {
            fixture.Register<ControllerContext, ActionResult, ResultExecutingContext>(
                (controllerContext, result) => new ResultExecutingContext(controllerContext, result));
        }

        private static void ResultExecutedContext(IFixture fixture)
        {
            fixture.Register<ControllerContext, ActionResult, bool, Exception, ResultExecutedContext>(
                (controllerContext, result, canceled, exception) => new ResultExecutedContext(controllerContext, result, canceled, exception));
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