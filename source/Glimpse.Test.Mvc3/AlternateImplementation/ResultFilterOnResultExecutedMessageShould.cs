using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ResultFilterOnResultExecutedMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ResultExecutedContext argument)
        {
            var sut = new ResultFilter.OnResultExecuted.Message(argument);

            Assert.Equal(argument.Canceled, sut.Canceled);
            Assert.Equal(argument.ExceptionHandled, sut.ExceptionHandled);
            Assert.Equal(argument.Exception.GetType(), sut.ExceptionType);
            Assert.Equal(argument.Result.GetType(), sut.ResultType);
        }

        [Theory, AutoMock]
        public void HandleNullExceptions(ResultExecutedContext argument)
        {
            argument.Exception = null;

            var sut = new ResultFilter.OnResultExecuted.Message(argument);

            Assert.Null(sut.ExceptionType);
        }

        [Theory, AutoMock]
        public void HandleNullResults(ResultExecutedContext argument)
        {
            argument.Result = null;

            var sut = new ResultFilter.OnResultExecuted.Message(argument);

            Assert.Null(sut.ResultType);
        }
    }
}