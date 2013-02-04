using System;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ExceptionFilterOnExceptionMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ExceptionContext context, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            var sut = new ExceptionFilter.OnException.Message(context, filterType, method, timerResult);

            Assert.Equal(context.ExceptionHandled, sut.ExceptionHandled);
            Assert.Equal(context.Exception.GetType(), sut.ExceptionType);
            Assert.Equal(context.Result.GetType(), sut.ResultType);
            Assert.Equal(filterType, sut.ExecutedType);
            Assert.Equal(method, sut.ExecutedMethod);
            Assert.Equal(timerResult.Duration, sut.Duration);
            Assert.Equal(timerResult.Offset, sut.Offset);
            Assert.Equal(FilterCategory.Exception, sut.Category);
        }

        [Theory, AutoMock]
        public void HandleNullValues(ExceptionContext context, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            context.Result = null;
            context.Exception = null;

            var sut = new ExceptionFilter.OnException.Message(context, filterType, method, timerResult);

            Assert.Null(sut.ExceptionType);
            Assert.Equal(typeof(EmptyResult), sut.ResultType);
        }
    }
}