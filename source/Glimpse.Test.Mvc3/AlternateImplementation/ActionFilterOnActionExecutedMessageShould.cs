using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionFilterOnActionExecutedMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ActionExecutedContext context)
        {
            var message = new ActionFilter.OnActionExecuted.Message(context);

            Assert.Equal(context.ActionDescriptor.ActionName, message.ActionName);
            Assert.Equal(context.ActionDescriptor.ControllerDescriptor.ControllerName, message.ControllerName);
            Assert.Equal(context.Exception.GetType(), message.ExceptionType);
            Assert.Equal(context.Canceled, message.IsCanceled);
            Assert.Equal(context.ExceptionHandled, message.ExceptionHandled);
            Assert.Equal(context.Result.GetType(), message.ResultType);
        }

        [Theory, AutoMock]
        public void ConstructWithMissingException(ActionExecutedContext context)
        {
            context.Exception = null;

            var message = new ActionFilter.OnActionExecuted.Message(context);

            Assert.Null(message.ExceptionType);
        }

        [Theory, AutoMock]
        public void ConstructWithMissingResult(ActionExecutedContext context)
        {
            context.Result = null;

            var message = new ActionFilter.OnActionExecuted.Message(context);

            Assert.NotNull(message.ResultType);
            Assert.Equal(typeof(EmptyResult), message.ResultType);
        }
    }
}