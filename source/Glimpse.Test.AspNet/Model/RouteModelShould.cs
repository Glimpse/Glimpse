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
            var defaults = new[] { new RouteDataItemModel { PlaceHolder = "controller", DefaultValue = "Home" } };
            var constraints = new[] { new RouteConstraintModel { IsMatch = true, ParameterName = "action", Constraint = ".+" } };
            var dataTokens = new RouteValueDictionary(new { area = "Test", name = "Hi" });
            var url = "{controller}/{action}/{id}";

            var test = new RouteModel();
            test.Area = "Test";
            test.Url = url;
            test.RouteData = defaults;
            test.Constraints = constraints;
            test.DataTokens = dataTokens;
             
            Assert.False(test.IsMatch);

            Assert.Equal("Test", test.Area);
            Assert.Equal(url, test.Url);
            Assert.Same(defaults, test.RouteData);
            Assert.Same(constraints, test.Constraints);
            Assert.Same(dataTokens, test.DataTokens);

            test.IsMatch = true;
            Assert.True(test.IsMatch); 
        }
    }
}