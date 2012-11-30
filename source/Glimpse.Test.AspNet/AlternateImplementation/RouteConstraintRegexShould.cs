using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

using Glimpse.AspNet.AlternateImplementation;

using Xunit;

namespace Glimpse.Test.AspNet.AlternateImplementation
{
    public class RouteConstraintRegexShould
    {
        [Fact]
        public void MatchValue()
        {
            var constraint = new RouteConstraintRegex("Test");
            var result = constraint.Match(null, null, "Param", new RouteValueDictionary { { "Param", "Test" }, { "OtherParam", "123" } }, RouteDirection.UrlGeneration);

            Assert.True(result);
        }
        
        [Fact]
        public void NotMatchValue()
        {
            var constraint = new RouteConstraintRegex("Test");
            var result = constraint.Match(null, null, "Param", new RouteValueDictionary { { "Param", "Other" }, { "OtherParam", "123" } }, RouteDirection.UrlGeneration);

            Assert.False(result);
        }
    }
}
