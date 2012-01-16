using System;
using Xunit;

namespace Glimpse.Test.AspNet.Model
{
    public class RouteInstanceShould:IDisposable
    {
        private RouteInstanceTester tester;

        public RouteInstanceTester RouteInstance
        {
            get { return tester ?? (tester = RouteInstanceTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            RouteInstance = null;
        }

        [Fact]
        public void Construct()
        {
            Assert.NotNull(RouteInstance.RouteType);
            Assert.False(RouteInstance.IsMatch);
        }

        [Fact]
        public void ReturnAreaName()
        {
            Assert.Equal("Test", RouteInstance.AreaName);
        }

        [Fact]
        public void ReturnDefaultAreaName()
        {
            RouteInstance.RouteTester.DataTokens.Clear();
            Assert.Equal("Root", RouteInstance.AreaName);
        }

        [Fact]
        public void ReturnConstraints()
        {
            Assert.Equal(RouteInstance.RouteTester.Constraints, RouteInstance.Constraints);
        }

        [Fact]
        public void ReturnDataTokens()
        {
            Assert.Equal(RouteInstance.RouteTester.DataTokens, RouteInstance.DataTokens);
        }

        [Fact]
        public void ReturnNullConstrainsWithOnlyRouteBase()
        {
            RouteInstance.Route = null;
            Assert.Null(RouteInstance.Constraints);
        }

        [Fact]
        public void ReturnNullDataTokensWithOnlyRouteBase()
        {
            RouteInstance.Route = null;
            Assert.Null(RouteInstance.DataTokens);
        }

        [Fact(Skip = "Not sure how to test this")]
        public void ReturnProperTokenInstances()
        {
            Assert.Equal(2, RouteInstance.UriTokens.Count);
        }

        [Fact]
        public void UriTemplateShouldMatch()
        {
            Assert.Equal("{controller}/{action}/{id}", RouteInstance.UriTemplate);
            
        }
    }
}