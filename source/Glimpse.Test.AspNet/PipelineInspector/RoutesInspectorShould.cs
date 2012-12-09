using System;
using System.Collections.Generic;
using System.Diagnostics; 
using System.Web; 
using Glimpse.AspNet.PipelineInspector;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework; 
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;
using MvcRoute = System.Web.Routing.Route;
using MvcRouteBase = System.Web.Routing.RouteBase;
using MvcRouteTable = System.Web.Routing.RouteTable;

namespace Glimpse.Test.AspNet.PipelineInspector
{
    public class RoutesInspectorShould
    {
        [Fact]
        public void Construct()
        {
            var sut = new RoutesInspector();

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IPipelineInspector>(sut);
        }
         
        [Theory, AutoMock]
        public void IntergrationTestRouteProxing(System.Web.Routing.IRouteHandler routeHandler, RoutesInspector sut, IPipelineInspectorContext context)
        {
            MvcRouteTable.Routes.Clear();
            MvcRouteTable.Routes.Add("Test", new MvcRoute("Test", routeHandler));
            MvcRouteTable.Routes.Add("BaseTyped", new NewRouteBase());
            MvcRouteTable.Routes.Add("BaseTestTyped", new NewConstructorRouteBase("Name"));
            MvcRouteTable.Routes.Add("SubTyped", new NewRoute("test", routeHandler));
            MvcRouteTable.Routes.Add("SubTestTyped", new NewConstructorRoute("test", routeHandler, "Name"));
            MvcRouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}", new { resource = "Test", pathInfo = "[0-9]" });

            context.Setup(x => x.ProxyFactory).Returns(new CastleDynamicProxyFactory(context.Logger, context.MessageBroker, () => new ExecutionTimer(new Stopwatch()), () => new RuntimePolicy()));

            sut.Setup(context);

            // This test needs to be like this because IProxyTargetAccessor is in Moq and Glimpse
            foreach (var route in MvcRouteTable.Routes)
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
        public void ExtendsMvcRoutes(System.Web.Routing.IRouteHandler routeHandler, RoutesInspector sut, IPipelineInspectorContext context, MvcRoute newRoute)
        {
            MvcRouteTable.Routes.Clear();
            MvcRouteTable.Routes.Add("Test", new MvcRoute("Test", routeHandler));

            context.ProxyFactory.Setup(x => x.ExtendClass<MvcRoute>(It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<IEnumerable<object>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newRoute, MvcRouteTable.Routes[0]);
        }

        [Theory, AutoMock]
        public void WrapsMvcRouteDerivedTypes(System.Web.Routing.IRouteHandler routeHandler, RoutesInspector sut, IPipelineInspectorContext context, NewRoute route, MvcRoute newRoute)
        {
            MvcRouteTable.Routes.Clear();
            MvcRouteTable.Routes.Add("Test", route);

            context.ProxyFactory.Setup(x => x.IsWrapClassEligible(typeof(MvcRoute))).Returns(true);
            context.ProxyFactory.Setup(x => x.WrapClass((MvcRoute)route, It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<IEnumerable<object>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newRoute, MvcRouteTable.Routes[0]);
        }

        [Theory, AutoMock]
        public void WrapsMvcRouteBaseDerivedTypes(System.Web.Routing.IRouteHandler routeHandler, RoutesInspector sut, IPipelineInspectorContext context, NewRouteBase route, MvcRouteBase newRoute)
        {
            MvcRouteTable.Routes.Clear();
            MvcRouteTable.Routes.Add("Test", route);

            context.ProxyFactory.Setup(x => x.IsWrapClassEligible(typeof(MvcRouteBase))).Returns(true);
            context.ProxyFactory.Setup(x => x.WrapClass((MvcRouteBase)route, It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newRoute, MvcRouteTable.Routes[0]);
        }

        public class NewRouteBase : MvcRouteBase
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

        public class NewRoute : MvcRoute
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

        public class NewConstructorRoute : MvcRoute
        {
            public NewConstructorRoute(string url, System.Web.Routing.IRouteHandler routeHandler, string name)
                : base(url, routeHandler)
            {
            }
        }
    }
}
