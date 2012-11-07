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
    public class ResultFilterOnResultExecutingMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ResultExecutingContext context, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            var sut = new ResultFilter.OnResultExecuting.Message(context, filterType, method, timerResult);

            Assert.Equal(context.Cancel, sut.IsCanceled);
            Assert.Equal(context.Result.GetType(), sut.ResultType);
        }

        [Theory, AutoMock]
        public void HandleNullResults(ResultExecutingContext context, Type filterType, MethodInfo method, TimerResult timerResult)
        {
            context.Result = null;

            var sut = new ResultFilter.OnResultExecuting.Message(context, filterType, method, timerResult);

            Assert.Null(sut.ResultType);
        }
    }
}