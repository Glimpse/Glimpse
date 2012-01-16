using System.Web;
using System.Web.Routing;
using Glimpse.AspNet.Model;
using Moq;

namespace Glimpse.Test.AspNet.Model
{
    public class RouteInstanceTester : RouteInstance
    {
        public Route RouteTester { get; set; }
        public Mock<HttpContextBase> HttpContextMock { get; set; }
        public Mock<IRouteHandler> RouteHandlerMock { get; set; }

        private RouteInstanceTester(Route route, Mock<HttpContextBase> httpContextMock, Mock<IRouteHandler> routeHandlerMock):base(route, httpContextMock.Object)
        {
            RouteTester = route;
            HttpContextMock = httpContextMock;
            RouteHandlerMock = routeHandlerMock;
        }

        public static RouteInstanceTester Create()
        {
            var routeHandlerMock = new Mock<IRouteHandler>();
            var route = new Route("{controller}/{action}/{id}", routeHandlerMock.Object)
                            {
                                Constraints = new RouteValueDictionary(new { action = ".+" }),
                                DataTokens = new RouteValueDictionary(new { area = "Test", name = "Hi" }),
                                Defaults = new RouteValueDictionary(new { controller = "home" })
                            };


            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/is/something");
            
            return new RouteInstanceTester(route, httpContextMock, routeHandlerMock);
        }
    }
}