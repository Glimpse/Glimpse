using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ExceptionFilterOnExceptionMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ExceptionContext context)
        {
            var sut = new ExceptionFilter.OnException.Message(context);

            Assert.Equal(context.ExceptionHandled, sut.ExceptionHandled);
            Assert.Equal(context.Exception.GetType(), sut.ExceptionType);
            Assert.Equal(context.Result.GetType(), sut.ResultType);
        }

        [Theory, AutoMock]
        public void HandleNullValues(ExceptionContext context)
        {
            context.Result = null;
            context.Exception = null;

            var sut = new ExceptionFilter.OnException.Message(context);

            Assert.Null(sut.ExceptionType);
            Assert.Equal(typeof(EmptyResult), sut.ResultType);
        }
    }
}