using System.Web.Routing;
using Glimpse.WebApi.AlternateType;
using Xunit;

namespace Glimpse.Test.WebApi.AlternateType
{
    public class IHttpRouteConstraintRegexShould
    {
        [Fact]
        public void MatchValue()
        {
            var constraint = new IHttpRouteConstraintRegex("Test");
            var result = constraint.Match(null, null, "Param", new System.Web.Http.Routing.HttpRouteValueDictionary { { "Param", "Test" }, { "OtherParam", "123" } }, System.Web.Http.Routing.HttpRouteDirection.UriGeneration);

            Assert.True(result);
        }
        
        [Fact]
        public void NotMatchValue()
        {
            var constraint = new IHttpRouteConstraintRegex("Test");
            var result = constraint.Match(null, null, "Param", new System.Web.Http.Routing.HttpRouteValueDictionary { { "Param", "Other" }, { "OtherParam", "123" } }, System.Web.Http.Routing.HttpRouteDirection.UriGeneration);

            Assert.False(result);
        }
    }
}
