using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Glimpse.AspNet.AlternateType;
using Glimpse.AspNet.Inspector;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Tab;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;
using Glimpse.Test.Common;
using Glimpse.Test.AspNet.Inspector;
using Moq;
using Xunit;
using Xunit.Extensions; 

namespace Glimpse.Test.AspNet.Tab
{ 
    public class RoutesShould
    {
        [Theory, AutoMock]
        public void ReturnName(Routes tab)
        {
            Assert.Equal("Routes", tab.Name);
        }

        [Theory, AutoMock]
        public void ReturnDocumentationUri(Routes tab)
        {
            Assert.True(tab.DocumentationUri.Contains("getGlimpse.com"));
        }

        [Theory, AutoMock]
        public void ReturnRouteInstancesEvenWhenContextIsNull(Routes tab, ITabContext context, RoutesInspector routeInspector, IInspectorContext routeInspectorContext)
        {
            context.Setup(x => x.GetRequestContext<HttpContextBase>()).Returns((HttpContextBase)null);

            System.Web.Routing.RouteTable.Routes.Clear();
            System.Web.Routing.RouteTable.Routes.Ignore("Test");
             
            routeInspectorContext.Setup(x => x.ProxyFactory).Returns(new CastleDynamicProxyFactory(routeInspectorContext.Logger, routeInspectorContext.MessageBroker, () => new ExecutionTimer(new Stopwatch()), () => new RuntimePolicy()));
            routeInspector.Setup(routeInspectorContext);

            var data = tab.GetData(context) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Equal(System.Web.Routing.RouteTable.Routes.Count, data.Count); 
        }

        [Theory, AutoMock]
        public void ReturnRouteInstancesEvenWhenRoutesTableEmpty(Routes tab, ITabContext context, RoutesInspector routeInspector, IInspectorContext routeInspectorContext)
        {
            System.Web.Routing.RouteTable.Routes.Clear();

            routeInspectorContext.Setup(x => x.ProxyFactory).Returns(new CastleDynamicProxyFactory(routeInspectorContext.Logger, routeInspectorContext.MessageBroker, () => new ExecutionTimer(new Stopwatch()), () => new RuntimePolicy()));
            routeInspector.Setup(routeInspectorContext);

            var data = tab.GetData(context) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Theory, AutoMock]
        public void ReturnProperNumberOfInstances(Routes tab, ITabContext context, RoutesInspector routeInspector, IInspectorContext routeInspectorContext)
        {
            System.Web.Routing.RouteTable.Routes.Clear();
            System.Web.Routing.RouteTable.Routes.Ignore("Something");

            routeInspectorContext.Setup(x => x.ProxyFactory).Returns(new CastleDynamicProxyFactory(routeInspectorContext.Logger, routeInspectorContext.MessageBroker, () => new ExecutionTimer(new Stopwatch()), () => new RuntimePolicy()));
            routeInspector.Setup(routeInspectorContext);
            
            var data = tab.GetData(context) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Equal(System.Web.Routing.RouteTable.Routes.Count, data.Count);
        }

        [Theory, AutoMock]
        public void SubscribeToConstraintMessageTypes(Routes tab, ITabSetupContext setupContext)
        { 
            tab.Setup(setupContext);

            setupContext.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<RouteBase.ProcessConstraint.Message>>()));
            setupContext.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<RouteBase.GetRouteData.Message>>())); 
        }

        [Theory, AutoMock]
        public void MatchConstraintMessageToRoute(Routes tab, ITabContext context, System.Web.Routing.IRouteConstraint constraint, RoutesInspector routeInspector, IInspectorContext routeInspectorContext)
        {
            var route = new System.Web.Routing.Route("url", new System.Web.Routing.RouteValueDictionary { { "Test", "Other" } }, new System.Web.Routing.RouteValueDictionary { { "Test", constraint } }, new System.Web.Routing.RouteValueDictionary { { "Data", "Tokens" } }, new System.Web.Routing.PageRouteHandler("~/Path"));

            System.Web.Routing.RouteTable.Routes.Clear();
            System.Web.Routing.RouteTable.Routes.Add(route);

            routeInspectorContext.Setup(x => x.ProxyFactory).Returns(new CastleDynamicProxyFactory(routeInspectorContext.Logger, routeInspectorContext.MessageBroker, () => new ExecutionTimer(new Stopwatch()), () => new RuntimePolicy()));
            routeInspector.Setup(routeInspectorContext);

            route = (System.Web.Routing.Route) System.Web.Routing.RouteTable.Routes[0];

            var routeMessage = new RouteBase.GetRouteData.Message(route.GetHashCode(), new System.Web.Routing.RouteData(), "routeName")
                .AsSourceMessage(route.GetType(), null)
                .AsTimedMessage(new TimerResult { Duration = TimeSpan.FromMilliseconds(19) });
            var constraintMessage = new RouteBase.ProcessConstraint.Message(new RouteBase.ProcessConstraint.Arguments(new object[] { (HttpContextBase)null, constraint, "test", (System.Web.Routing.RouteValueDictionary)null, System.Web.Routing.RouteDirection.IncomingRequest }), route.GetHashCode(), true)
                .AsTimedMessage(new TimerResult { Duration = TimeSpan.FromMilliseconds(25) })
                .AsSourceMessage(route.GetType(), null);

            context.TabStore.Setup(mb => mb.Contains(typeof(IList<RouteBase.ProcessConstraint.Message>).AssemblyQualifiedName)).Returns(true).Verifiable();
            context.TabStore.Setup(mb => mb.Contains(typeof(IList<RouteBase.GetRouteData.Message>).AssemblyQualifiedName)).Returns(true).Verifiable();

            context.TabStore.Setup(mb => mb.Get(typeof(IList<RouteBase.ProcessConstraint.Message>).AssemblyQualifiedName)).Returns(new List<RouteBase.ProcessConstraint.Message> { constraintMessage }).Verifiable();
            context.TabStore.Setup(mb => mb.Get(typeof(IList<RouteBase.GetRouteData.Message>).AssemblyQualifiedName)).Returns(new List<RouteBase.GetRouteData.Message> { routeMessage }).Verifiable();
             
            var model = tab.GetData(context) as List<RouteModel>;
            var itemModel = model[0];
             
            Assert.NotNull(model);
            Assert.Equal(1, model.Count);
            Assert.NotNull(itemModel.Constraints);
            Assert.True(itemModel.IsMatch);
            Assert.Equal("Test", ((List<RouteConstraintModel>)itemModel.Constraints)[0].ParameterName);
            Assert.Equal(true, ((List<RouteConstraintModel>)itemModel.Constraints)[0].IsMatch);
            Assert.NotNull(itemModel.DataTokens);
            Assert.Equal("Tokens", itemModel.DataTokens["Data"]);
            Assert.NotNull(itemModel.RouteData);
            Assert.Equal("Other", ((List<RouteDataItemModel>)itemModel.RouteData)[0].DefaultValue);
        }
        
        [Theory, AutoMock]
        public void ReturnAspNetProxiedRouteInstances(Routes tab, ITabContext context, RoutesInspector routeInspector, IInspectorContext routeInspectorContext, System.Web.Routing.IRouteHandler routeHandler)
        {
            System.Web.Routing.RouteTable.Routes.Clear();
            System.Web.Routing.RouteTable.Routes.Add("Test", new System.Web.Routing.Route("Test", routeHandler));
            System.Web.Routing.RouteTable.Routes.Add("BaseTyped", new RoutesInspectorShould.NewRouteBase());
            System.Web.Routing.RouteTable.Routes.Add("BaseTestTyped", new RoutesInspectorShould.NewConstructorRouteBase("Name"));
            System.Web.Routing.RouteTable.Routes.Add("SubTyped", new RoutesInspectorShould.NewRoute("test", routeHandler));
            System.Web.Routing.RouteTable.Routes.Add("SubTestTyped", new RoutesInspectorShould.NewConstructorRoute("test", routeHandler, "Name"));
            System.Web.Routing.RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}", new { resource = "Test", pathInfo = "[0-9]" });

            routeInspectorContext.Setup(x => x.ProxyFactory).Returns(new CastleDynamicProxyFactory(routeInspectorContext.Logger, routeInspectorContext.MessageBroker, () => new ExecutionTimer(new Stopwatch()), () => new RuntimePolicy()));
            routeInspector.Setup(routeInspectorContext);

            var model = tab.GetData(context) as List<RouteModel>;
             
            Assert.NotNull(model);
            Assert.Equal(6, model.Count);
        }
    }
}