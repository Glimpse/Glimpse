using System;
using Glimpse.Core;
using Glimpse.Core.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.PipelineInspector;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.PipelineInspector
{
    public class RoutesInspectorShould
    {
        public RoutesInspectorShould()
        {
            var routeHandler = new Mock<System.Web.Routing.IRouteHandler>().Object;

            System.Web.Routing.RouteTable.Routes.Clear();
            System.Web.Routing.RouteTable.Routes.Add("Test", new System.Web.Routing.Route("Test", routeHandler));
            System.Web.Routing.RouteTable.Routes.Add("Other", new System.Web.Routing.Route("Other", routeHandler));
        }

        [Fact]
        public void Construct()
        {
            var sut = new RoutesInspector();

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IPipelineInspector>(sut);
        }

        [Theory, AutoMock]
        public void Setup(RoutesInspector sut, IPipelineInspectorContext context, System.Web.Routing.Route route1, System.Web.Routing.Route route2)
        { 
            context.ProxyFactory.Setup(pf => pf.IsProxyable(It.IsAny<object>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.CreateProxy((System.Web.Routing.Route)System.Web.Routing.RouteTable.Routes[0], It.IsAny<IEnumerable<IAlternateImplementation<System.Web.Routing.Route>>>(), null, It.IsAny<object[]>())).Returns(route1);
            context.ProxyFactory.Setup(pf => pf.CreateProxy((System.Web.Routing.Route)System.Web.Routing.RouteTable.Routes[1], It.IsAny<IEnumerable<IAlternateImplementation<System.Web.Routing.Route>>>(), null, It.IsAny<object[]>())).Returns(route2);

            sut.Setup(context);

            context.ProxyFactory.Verify(pf => pf.CreateProxy(It.IsAny<System.Web.Routing.Route>(), It.IsAny<IEnumerable<IAlternateImplementation<System.Web.Routing.Route>>>(), null, It.IsAny<object[]>()), Times.AtLeastOnce());
        }

        [Theory, AutoMock]
        public void SetupRealProxy(RoutesInspector sut, IPipelineInspectorContext context, System.Web.Routing.Route route1, System.Web.Routing.Route route2)
        {
            var routeUrl1 = GetRouteUrl(0);
            var routeUrl2 = GetRouteUrl(1);

            var proxyFactory = new CastleDynamicProxyFactory(context.Logger, context.MessageBroker, () => null, () => RuntimePolicy.On);
            context.Setup(x => x.ProxyFactory).Returns(proxyFactory);

            sut.Setup(context);
            
            Assert.Equal(routeUrl1, GetRouteUrl(0));
            Assert.Equal(routeUrl2, GetRouteUrl(1));
        }

        private string GetRouteUrl(int index)
        {
            return ((System.Web.Routing.Route)System.Web.Routing.RouteTable.Routes[index]).Url;
        }
    }
}
