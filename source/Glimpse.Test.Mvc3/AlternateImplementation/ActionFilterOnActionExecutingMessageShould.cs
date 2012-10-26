using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionFilterOnActionExecutingMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ActionExecutingContext context)
        {
            var message = new ActionFilter.OnActionExecuting.Message(context);

            Assert.Equal(context.ActionDescriptor.ActionName, message.ActionName);
            Assert.Equal(context.Result.GetType(), message.ResultType);
        }
    }
}