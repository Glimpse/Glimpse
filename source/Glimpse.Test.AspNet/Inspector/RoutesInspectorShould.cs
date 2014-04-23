using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Routing;
using Glimpse.AspNet.AlternateType;
using Glimpse.AspNet.Inspector;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;
using RouteBase = System.Web.Routing.RouteBase;

namespace Glimpse.Test.AspNet.Inspector
{
    public class RoutesInspectorShould
    {
        [Fact]
        public void Construct()
        {
            var sut = new RoutesInspector();

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IInspector>(sut);
        }
         
        [Theory, AutoMock]
        public void IntergrationTestRouteProxing(RoutesInspector sut, System.Web.Routing.IRouteHandler routeHandler, IInspectorContext context)
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.Add("Test", new System.Web.Routing.Route("Test", routeHandler));
            RouteTable.Routes.Add("BaseTyped", new NewRouteBase());
            RouteTable.Routes.Add("BaseTestTyped", new NewConstructorRouteBase("Name"));
            RouteTable.Routes.Add("SubTyped", new NewRoute("test", routeHandler));
            RouteTable.Routes.Add("SubTestTyped", new NewConstructorRoute("test", routeHandler, "Name"));
            RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}", new { resource = "Test", pathInfo = "[0-9]" });

            context.Setup(x => x.ProxyFactory).Returns(new CastleDynamicProxyFactory(context.Logger, context.MessageBroker, () => new ExecutionTimer(new Stopwatch()), () => new RuntimePolicy()));

            sut.Setup(context);

            // This test needs to be like this because IProxyTargetAccessor is in Moq and Glimpse
            foreach (var route in RouteTable.Routes)
            {
                var found = false;
                foreach (var routeInterface in route.GetType().GetInterfaces())
                {
                    if (routeInterface.Name == "IProxyTargetAccessor")
                    {
                        found = true;
                    }
                }

                Assert.True(found);
            }
        }

        [Theory, AutoMock]
        public void ExtendsMvcRoutes(System.Web.Routing.IRouteHandler routeHandler, RoutesInspector sut, IInspectorContext context, System.Web.Routing.Route newRoute)
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.Add("Test", new System.Web.Routing.Route("Test", routeHandler));

            context.ProxyFactory.Setup(x => x.ExtendClass<System.Web.Routing.Route>(It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<IEnumerable<object>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newRoute, RouteTable.Routes[0]);
        }

        [Theory, AutoMock]
        public void WrapsMvcRouteDerivedTypes(RoutesInspector sut, System.Web.Routing.IRouteHandler routeHandler, IInspectorContext context, NewRoute route, System.Web.Routing.Route newRoute)
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.Add("Test", route);

            context.ProxyFactory.Setup(x => x.IsWrapClassEligible(typeof(System.Web.Routing.Route))).Returns(true).Verifiable();
            context.ProxyFactory.Setup(x => x.WrapClass((System.Web.Routing.Route)route, It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<IEnumerable<object>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newRoute, RouteTable.Routes[0]);
        }

        [Theory, AutoMock]
        public void WrapsMvcRouteBaseDerivedTypes(RoutesInspector sut, System.Web.Routing.IRouteHandler routeHandler, IInspectorContext context, NewRouteBase route, RouteBase newRoute)
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.Add("Test", route);

            context.ProxyFactory.Setup(x => x.IsWrapClassEligible(typeof(RouteBase))).Returns(true);
            context.ProxyFactory.Setup(x => x.WrapClass((RouteBase)route, It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newRoute, RouteTable.Routes[0]);
        }

        [Theory, AutoMock]
        public void ExtendsStringConstraints(RoutesInspector sut, IInspectorContext context, NewRoute route, System.Web.Routing.Route newRoute, string routeConstraint)
        {
            route.Constraints = new RouteValueDictionary { { "controller", routeConstraint } };

            RouteTable.Routes.Clear();
            RouteTable.Routes.Add("Test", route);

            context.ProxyFactory.Setup(x => x.IsWrapClassEligible(typeof(System.Web.Routing.Route))).Returns(true).Verifiable();
            context.ProxyFactory.Setup(x => x.WrapClass((System.Web.Routing.Route)route, It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<IEnumerable<object>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(typeof(RouteConstraintRegex), route.Constraints["controller"].GetType());
        }
         
        [Theory, AutoMock]
        public void ExtendsRouteConstraintConstraints(RoutesInspector sut, IInspectorContext context, NewRoute route, System.Web.Routing.Route newRoute, IRouteConstraint routeConstraint, IRouteConstraint newRouteConstraint)
        {
            route.Constraints = new RouteValueDictionary { { "controller", routeConstraint } };

            RouteTable.Routes.Clear();
            RouteTable.Routes.Add("Test", route);

            context.ProxyFactory.Setup(x => x.IsWrapClassEligible(typeof(System.Web.Routing.Route))).Returns(true).Verifiable();
            context.ProxyFactory.Setup(x => x.WrapClass((System.Web.Routing.Route)route, It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<IEnumerable<object>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();
            context.ProxyFactory.Setup(x => x.IsWrapInterfaceEligible<IRouteConstraint>(typeof(IRouteConstraint))).Returns(true).Verifiable();
            context.ProxyFactory.Setup(x => x.WrapInterface(routeConstraint, It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<IEnumerable<object>>())).Returns(newRouteConstraint).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newRouteConstraint, route.Constraints["controller"]);
        }


        public class NewRouteBase : RouteBase
        {
            public override System.Web.Routing.RouteData GetRouteData(HttpContextBase httpContext)
            { 
                return new System.Web.Routing.RouteData();
            }

            public override System.Web.Routing.VirtualPathData GetVirtualPath(System.Web.Routing.RequestContext requestContext, System.Web.Routing.RouteValueDictionary values)
            {
                return new System.Web.Routing.VirtualPathData(this, "Test");
            }
        }

        public class NewConstructorRouteBase : NewRouteBase
        {
            public NewConstructorRouteBase(string name)
            {
            }
        }

        public class NewRoute : System.Web.Routing.Route
        {
            public NewRoute(string url, System.Web.Routing.IRouteHandler routeHandler)
                : base(url, routeHandler)
            {
            }

            public NewRoute(string url, System.Web.Routing.RouteValueDictionary defaults, System.Web.Routing.IRouteHandler routeHandler)
                : base(url, defaults, routeHandler)
            {
            }

            public NewRoute(string url, System.Web.Routing.RouteValueDictionary defaults, System.Web.Routing.RouteValueDictionary constraints, System.Web.Routing.IRouteHandler routeHandler)
                : base(url, defaults, constraints, routeHandler)
            {
            }

            public NewRoute(string url, System.Web.Routing.RouteValueDictionary defaults, System.Web.Routing.RouteValueDictionary constraints, System.Web.Routing.RouteValueDictionary dataTokens, System.Web.Routing.IRouteHandler routeHandler)
                : base(url, defaults, constraints, dataTokens, routeHandler)
            {
            }
        }

        public class NewConstructorRoute : System.Web.Routing.Route
        {
            public NewConstructorRoute(string url, System.Web.Routing.IRouteHandler routeHandler, string name)
                : base(url, routeHandler)
            {
            }
        }
    }
}
