using System;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class AuthorizationFilterOnAuthorizationMessageShould
    {
        [Theory, AutoMock]
        public void Constuct(AuthorizationContext argument, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            var sut = new AuthorizationFilter.OnAuthorization.Message(argument, filterType, method, timerResult);

            Assert.Equal(argument.ActionDescriptor.ActionName, sut.ActionName);
            Assert.Equal(argument.Result.GetType(), sut.ResultType);
            Assert.Equal(filterType, sut.ExecutedType);
            Assert.Equal(method, sut.ExecutedMethod);
            Assert.Equal(timerResult.Duration, sut.Duration);
            Assert.Equal(timerResult.Offset, sut.Offset);
            Assert.Contains(sut.ActionName, sut.EventName);
            Assert.Contains(sut.ControllerName, sut.EventName);
            Assert.Equal(FilterCategory.Authorization, sut.Category);
        }

        [Theory, AutoMock]
        public void HandleNullResults(AuthorizationContext argument, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            argument.Result = null;

            var sut = new AuthorizationFilter.OnAuthorization.Message(argument, filterType, method, timerResult);

            Assert.Null(sut.ResultType);
        }
    }
}