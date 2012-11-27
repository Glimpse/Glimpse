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
            var constraints = new[] { new RouteConstraintModel { Checked = true, Matched = true, ParameterName = "action", Constraint = ".+" } };
            var dataTokens = new RouteValueDictionary(new { area = "Test", name = "Hi" });
            var url = "{controller}/{action}/{id}";

            var test = new RouteModel();
            test.Area = "Test";
            test.Url = url;
            test.RouteData = defaults;
            test.Constraints = constraints;
            test.DataTokens = dataTokens;

            Assert.False(test.IsFirstMatch);
            Assert.False(test.MatchesCurrentRequest);

            Assert.Equal("Test", test.Area);
            Assert.Equal(url, test.Url);
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