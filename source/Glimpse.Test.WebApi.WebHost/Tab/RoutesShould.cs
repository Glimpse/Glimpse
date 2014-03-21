using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.WebApi.Core.AlternateType;
using Glimpse.WebApi.Core.Model;
using Glimpse.WebApi.Tab;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Test.Common; 
using Moq;
using Xunit;
using Xunit.Extensions;
using System.Collections.Specialized;

namespace Glimpse.Test.WebApi.WebHost.Tab
{ 
    public class RoutesShould
    {
        [Theory, AutoMock]
        public void ReturnName(Routes tab)
        {
            Assert.Equal("WebAPI Routes", tab.Name);
        }

        [Theory, AutoMock]
        public void ReturnDocumentationUri(Routes tab)
        {
            Assert.True(tab.DocumentationUri.Contains("getGlimpse.com"));
        }

        [Theory, AutoMock]
        public void ReturnRouteInstancesEvenWhenContextIsNull(Routes tab, ITabContext context)
        {
            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.Items["MS_HttpRequestMessage"]).Returns(null);
            context.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

            System.Web.Http.GlobalConfiguration.Configuration.Routes.Clear();
             
            var data = tab.GetData(context) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Equal(System.Web.Http.GlobalConfiguration.Configuration.Routes.Count, data.Count); 
        }

        [Theory, AutoMock]
        public void ReturnRouteInstancesEvenWhenRoutesTableEmpty(Routes tab, ITabContext context)
        {
            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.Items["MS_HttpRequestMessage"]).Returns(null);
            context.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

            System.Web.Http.GlobalConfiguration.Configuration.Routes.Clear();

            var data = tab.GetData(context) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Theory, AutoMock]
        public void ReturnProperNumberOfInstances(Routes tab, ITabContext context)
        {
            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.Items["MS_HttpRequestMessage"]).Returns(null);
            context.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

            System.Web.Http.GlobalConfiguration.Configuration.Routes.Clear();

            var data = tab.GetData(context) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Equal(System.Web.Http.GlobalConfiguration.Configuration.Routes.Count, data.Count);
        }

        [Theory, AutoMock]
        public void SubscribeToConstraintMessageTypes(Routes tab, ITabSetupContext setupContext)
        { 
            tab.Setup(setupContext);

            setupContext.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<IHttpRoute.ProcessConstraint.Message>>()));
            setupContext.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<IHttpRoute.GetRouteData.Message>>())); 
        }

        [Theory, AutoMock]
        public void MatchConstraintMessageToRoute(Routes tab, ITabContext context, System.Web.Routing.IRouteConstraint constraint)
        {
            var route = new System.Web.Http.Routing.HttpRoute("url", new System.Web.Http.Routing.HttpRouteValueDictionary { { "Test", "Other" } }, new System.Web.Http.Routing.HttpRouteValueDictionary { { "Test", constraint } }, new System.Web.Http.Routing.HttpRouteValueDictionary { { "Data", "Tokens" } }, new System.Net.Http.HttpClientHandler());

            System.Web.Http.GlobalConfiguration.Configuration.Routes.Clear();
            System.Web.Http.GlobalConfiguration.Configuration.Routes.Add("route", route); 

            var routeMessage = new IHttpRoute.GetRouteData.Message(route.GetHashCode(), new System.Web.Http.Routing.HttpRouteData(route), "routeName")
                .AsSourceMessage(route.GetType(), null)
                .AsTimedMessage(new TimerResult { Duration = TimeSpan.FromMilliseconds(19) });
            var constraintMessage = new IHttpRoute.ProcessConstraint.Message(new IHttpRoute.ProcessConstraint.Arguments(new object[] { (HttpContextBase)null, constraint, "test", (System.Web.Http.Routing.HttpRouteValueDictionary)null, System.Web.Http.Routing.HttpRouteDirection.UriGeneration }), route.GetHashCode(), true)
                .AsTimedMessage(new TimerResult { Duration = TimeSpan.FromMilliseconds(25) })
                .AsSourceMessage(route.GetType(), null);

            context.TabStore.Setup(mb => mb.Contains(typeof(IList<IHttpRoute.ProcessConstraint.Message>).AssemblyQualifiedName)).Returns(true).Verifiable();
            context.TabStore.Setup(mb => mb.Contains(typeof(IList<IHttpRoute.GetRouteData.Message>).AssemblyQualifiedName)).Returns(true).Verifiable();

            context.TabStore.Setup(mb => mb.Get(typeof(IList<IHttpRoute.ProcessConstraint.Message>).AssemblyQualifiedName)).Returns(new List<IHttpRoute.ProcessConstraint.Message> { constraintMessage }).Verifiable();
            context.TabStore.Setup(mb => mb.Get(typeof(IList<IHttpRoute.GetRouteData.Message>).AssemblyQualifiedName)).Returns(new List<IHttpRoute.GetRouteData.Message> { routeMessage }).Verifiable();

            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.Items["MS_HttpRequestMessage"]).Returns(null);
            context.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

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