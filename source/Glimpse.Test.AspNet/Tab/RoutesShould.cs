using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.AspNet.AlternateType;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Tab;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common; 
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
        public void ReturnRouteInstancesEvenWhenContextIsNull(Routes tab, ITabContext context)
        {
            context.Setup(x => x.GetRequestContext<HttpContextBase>()).Returns((HttpContextBase)null);

            System.Web.Routing.RouteTable.Routes.Clear();
            System.Web.Routing.RouteTable.Routes.Ignore("Test");
             
            var data = tab.GetData(context) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Equal(System.Web.Routing.RouteTable.Routes.Count, data.Count); 
        }

        [Theory, AutoMock]
        public void ReturnRouteInstancesEvenWhenRoutesTableEmpty(Routes tab, ITabContext context)
        {
            System.Web.Routing.RouteTable.Routes.Clear();

            var data = tab.GetData(context) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Theory, AutoMock]
        public void ReturnProperNumberOfInstances(Routes tab, ITabContext context)
        {
            System.Web.Routing.RouteTable.Routes.Clear();
            System.Web.Routing.RouteTable.Routes.Ignore("Something");

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
        public void MatchConstraintMessageToRoute(Routes tab, ITabContext context, System.Web.Routing.IRouteConstraint constraint)
        {
            var route = new System.Web.Routing.Route("url", new System.Web.Routing.RouteValueDictionary { { "Test", "Other" } }, new System.Web.Routing.RouteValueDictionary { { "Test", constraint } }, new System.Web.Routing.RouteValueDictionary { { "Data", "Tokens" } }, new System.Web.Routing.PageRouteHandler("~/Path"));

            System.Web.Routing.RouteTable.Routes.Clear();
            System.Web.Routing.RouteTable.Routes.Add(route); 

            var routeMessage = new RouteBase.GetRouteData.Message(new TimerResult { Duration = TimeSpan.FromMilliseconds(19) }, route.GetType(), null, route.GetHashCode(), new System.Web.Routing.RouteData(), "routeName");
            var constraintMessage = new RouteBase.ProcessConstraint.Message(new RouteBase.ProcessConstraint.Arguments(new object[] { (HttpContextBase)null, constraint, "test", (System.Web.Routing.RouteValueDictionary)null, System.Web.Routing.RouteDirection.IncomingRequest }), new TimerResult { Duration = TimeSpan.FromMilliseconds(25) }, route.GetType(), null, route.GetHashCode(), true);

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
    }
}