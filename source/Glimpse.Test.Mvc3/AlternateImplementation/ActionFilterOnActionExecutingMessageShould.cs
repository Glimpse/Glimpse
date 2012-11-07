using System;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionFilterOnActionExecutingMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ActionExecutingContext context, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            var sut = new ActionFilter.OnActionExecuting.Message(context, filterType, method, timerResult);

            Assert.Equal(context.ActionDescriptor.ActionName, sut.ActionName);
            Assert.Equal(context.Result.GetType(), sut.ResultType);
            Assert.Equal(filterType, sut.FilterType);
            Assert.Equal(method, sut.Method);
            Assert.Equal(timerResult.Duration, sut.Duration);
            Assert.Equal(timerResult.Offset, sut.Offset);
        }
    }
}