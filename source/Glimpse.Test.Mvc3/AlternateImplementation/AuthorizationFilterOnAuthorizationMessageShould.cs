using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class AuthorizationFilterOnAuthorizationMessageShould
    {
        [Theory, AutoMock]
        public void Constuct(AuthorizationContext argument)
        {
            var sut = new AuthorizationFilter.OnAuthorization.Message(argument);

            Assert.Equal(argument.ActionDescriptor.ActionName, sut.ActionName);
            Assert.Equal(argument.Result.GetType(), sut.ResultType);
        }

        [Theory, AutoMock]
        public void HandleNullResults(AuthorizationContext argument)
        {
            argument.Result = null;

            var sut = new AuthorizationFilter.OnAuthorization.Message(argument);

            Assert.Null(sut.ResultType);
        }
    }
}