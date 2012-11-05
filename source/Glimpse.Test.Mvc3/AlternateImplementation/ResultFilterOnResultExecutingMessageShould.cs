using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ResultFilterOnResultExecutingMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ResultExecutingContext context)
        {
            var sut = new ResultFilter.OnResultExecuting.Message(context);

            Assert.Equal(context.Cancel, sut.IsCanceled);
            Assert.Equal(context.Result.GetType(), sut.ResultType);
        }

        [Theory, AutoMock]
        public void HandleNullResults(ResultExecutingContext context)
        {
            context.Result = null;

            var sut = new ResultFilter.OnResultExecuting.Message(context);

            Assert.Null(sut.ResultType);
        }
    }
}