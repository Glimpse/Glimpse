using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Http.Routing;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Common;
using Glimpse.WebApi.Core.AlternateType;
using Glimpse.WebApi.WebHost.Inspector;
using Moq;
using Xunit;
using Xunit.Extensions;
using IHttpRoute = System.Web.Http.Routing.IHttpRoute;

namespace Glimpse.Test.WebApi.WebHost.Inspector
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
        public void IntergrationTestRouteProxing(RoutesInspector sut, System.Net.Http.HttpMessageHandler routeHandler, IInspectorContext context)
        {
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Clear();
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Add("Test", new HttpRoute());
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Add("InterfaceTyped", new NewIHttpRoute());
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Add("InterfaceTestTyped", new NewConstructorIHttpRoute());
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Add("SubTyped", new NewHttpRoute());
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Add("SubTestTyped", new NewConstructorHttpRoute());

            context.Setup(x => x.ProxyFactory).Returns(new CastleDynamicProxyFactory(context.Logger, context.MessageBroker, () => new ExecutionTimer(new Stopwatch()), () => new RuntimePolicy()));

            sut.Setup(context);
            
            Assert.Equal(5, System.Web.Http.GlobalConfiguration.Configuration.Routes.Count);

            // This test needs to be like this because IProxyTargetAccessor is in Moq and Glimpse
            foreach (var route in System.Web.Http.GlobalConfiguration.Configuration.Routes)
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
        public void ExtendsWebApiRoutes(System.Net.Http.HttpMessageHandler routeHandler, RoutesInspector sut, IInspectorContext context, HttpRoute newHttpRoute)
        {
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Clear();
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Add("Test", newHttpRoute);

            context.ProxyFactory.Setup(x => x.ExtendClass<HttpRoute>(It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<IEnumerable<object>>(), It.IsAny<object[]>())).Returns(newHttpRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newHttpRoute, System.Web.Http.GlobalConfiguration.Configuration.Routes[0]);
        }

        [Theory, AutoMock]
        public void WrapsWebApiRouteDerivedTypes(RoutesInspector sut, System.Net.Http.HttpMessageHandler routeHandler, IInspectorContext context, NewHttpRoute route, HttpRoute newRoute)
        {
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Clear();
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Add("Test", route);

            context.ProxyFactory.Setup(x => x.IsWrapClassEligible(typeof(HttpRoute))).Returns(true).Verifiable();
            context.ProxyFactory.Setup(x => x.WrapClass((HttpRoute)route, It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<IEnumerable<object>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newRoute, System.Web.Http.GlobalConfiguration.Configuration.Routes[0]);
        }

        [Theory, AutoMock]
        public void WrapsWebApiIHttpRouteDerivedTypes(RoutesInspector sut, System.Net.Http.HttpMessageHandler routeHandler, IInspectorContext context, NewIHttpRoute route, IHttpRoute newRoute)
        {
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Clear();
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Add("Test", route);

            context.ProxyFactory.Setup(x => x.IsWrapClassEligible(typeof(IHttpRoute))).Returns(true);
            context.ProxyFactory.Setup(x => x.WrapClass((IHttpRoute)route, It.IsAny<IEnumerable<IAlternateMethod>>(), It.IsAny<object[]>())).Returns(newRoute).Verifiable();

            sut.Setup(context);

            context.ProxyFactory.VerifyAll();
            Assert.Same(newRoute, System.Web.Http.GlobalConfiguration.Configuration.Routes[0]);
        }

        public class NewIHttpRoute : IHttpRoute
        {
            public System.Web.Http.Routing.IHttpRouteData GetRouteData(string virtualPathRoot, System.Net.Http.HttpRequestMessage request)
            { 
                return new System.Web.Http.Routing.HttpRouteData(this);
            }

            public System.Web.Http.Routing.IHttpVirtualPathData GetVirtualPath(System.Net.Http.HttpRequestMessage request, IDictionary<string, object> values)
            {
                return new System.Web.Http.Routing.HttpVirtualPathData(this, "Test");
            }
          
            public string RouteTemplate {
              get {
                return "TestRouteTemplate";
              }
            }
                  
            public IDictionary<string, object> Defaults {
              get {
                return new System.Web.Http.Routing.HttpRouteValueDictionary { { "Test", "Other" } };
              }
            }
                  
            public IDictionary<string, object> Constraints {
              get {
                return new System.Web.Http.Routing.HttpRouteValueDictionary { { "Test", "Constraint" } };
              }
            }
                  
            public IDictionary<string, object> DataTokens {
              get {
                return new System.Web.Http.Routing.HttpRouteValueDictionary { { "Data", "Tokens" } };
              }
            }
                  
            public System.Net.Http.HttpMessageHandler Handler {
              get {
                return new System.Net.Http.HttpClientHandler();
              }
            }
        }

        public class NewConstructorIHttpRoute : NewIHttpRoute
        {
            public NewConstructorIHttpRoute()
            {
            }
        }

        public class NewHttpRoute : HttpRoute
        {
            public NewHttpRoute()
                : base()
            {
            }
            
            public NewHttpRoute(string url)
                : base(url)
            {
            }
            
            public NewHttpRoute(string url, System.Web.Http.Routing.HttpRouteValueDictionary defaults)
                : base(url, defaults)
            {
            }

            public NewHttpRoute(string url, System.Web.Http.Routing.HttpRouteValueDictionary defaults, System.Web.Http.Routing.HttpRouteValueDictionary constraints)
                : base(url, defaults, constraints)
            {
            }

            public NewHttpRoute(string url, System.Web.Http.Routing.HttpRouteValueDictionary defaults, System.Web.Http.Routing.HttpRouteValueDictionary constraints, System.Web.Http.Routing.HttpRouteValueDictionary dataTokens)
                : base(url, defaults, constraints, dataTokens)
            {
            }

            public NewHttpRoute(string url, System.Web.Http.Routing.HttpRouteValueDictionary defaults, System.Web.Http.Routing.HttpRouteValueDictionary constraints, System.Web.Http.Routing.HttpRouteValueDictionary dataTokens, System.Net.Http.HttpMessageHandler messageHandler)
                : base(url, defaults, constraints, dataTokens, messageHandler)
            {
            }
        }

        public class NewConstructorHttpRoute : HttpRoute
        {
            public NewConstructorHttpRoute()
                : base()
            {
            }
        }
    }
}
