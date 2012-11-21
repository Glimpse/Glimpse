using System.Web.Routing;
using Glimpse.AspNet.Model;
using Xunit;

namespace Glimpse.Test.AspNet.Model
{
    public class RouteModelShould
    {
        [Fact]
        public void SetProperties()
        {
            var defaults = new[] { new RouteDataItemModel("controller", "Home") };
            var constraints = new[] { new RouteConstraintModel("action", ".+", true, true) };
            var dataTokens = new RouteValueDictionary(new { area = "Test", name = "Hi" });

            const string url = "{controller}/{action}/{id}";
            var test = new RouteModel("Test", url, defaults, constraints, dataTokens);
            
            Assert.False(test.IsFirstMatch);
            Assert.False(test.MatchesCurrentRequest);

            Assert.Equal("Test", test.Area);
            Assert.Equal(url, test.URL);
            Assert.Same(defaults, test.RouteData);
            Assert.Same(constraints, test.Constraints);
            Assert.Same(dataTokens, test.DataTokens);

            test.MatchesCurrentRequest = true;
            Assert.True(test.MatchesCurrentRequest);

            test.IsFirstMatch = true;
            Assert.True(test.IsFirstMatch);
        }
    }
}