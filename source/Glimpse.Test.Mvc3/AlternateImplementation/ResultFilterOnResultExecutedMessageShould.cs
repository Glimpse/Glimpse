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
    public class ResultFilterOnResultExecutedMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ResultExecutedContext argument, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            var sut = new ResultFilter.OnResultExecuted.Message(argument, filterType, method, timerResult);

            Assert.Equal(argument.Canceled, sut.Canceled);
            Assert.Equal(argument.ExceptionHandled, sut.ExceptionHandled);
            Assert.Equal(argument.Exception.GetType(), sut.ExceptionType);
            Assert.Equal(argument.Result.GetType(), sut.ResultType);
            Assert.Equal(filterType, sut.ExecutedType);
            Assert.Equal(method, sut.ExecutedMethod);
            Assert.Equal(timerResult.Duration, sut.Duration);
            Assert.Equal(timerResult.Offset, sut.Offset);
            Assert.Equal(FilterCategory.Result, sut.Category);
        }

        [Theory, AutoMock]
        public void HandleNullExceptions(ResultExecutedContext argument, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            argument.Exception = null;

            var sut = new ResultFilter.OnResultExecuted.Message(argument, filterType, method, timerResult);

            Assert.Null(sut.ExceptionType);
        }

        [Theory, AutoMock]
        public void HandleNullResults(ResultExecutedContext argument, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            argument.Result = null;

            var sut = new ResultFilter.OnResultExecuted.Message(argument, filterType, method, timerResult);

            Assert.Null(sut.ResultType);
        }
    }
}