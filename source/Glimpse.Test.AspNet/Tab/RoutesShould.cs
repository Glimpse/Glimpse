using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Tab;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;
using System.Collections.Generic;
using Route = Glimpse.AspNet.AlternateImplementation.Route;

namespace Glimpse.Test.AspNet.Tab
{
    public class RouteTester : Routes
    {
        public Mock<ITabContext> TabContextMock { get; set; }

        public Mock<HttpContextBase> HttpContextMock { get; set; }

        private RouteTester()
        {
            HttpContextMock = new Mock<HttpContextBase>();
            HttpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/is/something");

            TabContextMock = new Mock<ITabContext>();
            TabContextMock.Setup(tc => tc.GetRequestContext<HttpContextBase>()).Returns(HttpContextMock.Object);
        }

        public static RouteTester Create()
        {
            return new RouteTester();
        }
    }

    public class RoutesShould:IDisposable
    {
        private RouteTester tester;

        public RouteTester Tab
        {
            get { return tester ?? (tester = RouteTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Tab = null;
        }

        [Fact]
        public void ReturnName()
        {
            Assert.Equal("Routes", Tab.Name);
        }

        [Fact]
        public void ReturnDocumentationUri()
        {
            Assert.True(Tab.DocumentationUri.Contains("getGlimpse.com"));
        }

        [Fact(Skip = "Work in progress")]
        public void ReturnRouteInstancesEvenWhenContextIsNull()
        {
            Tab.TabContextMock.Setup(tc => tc.GetRequestContext<HttpContextBase>()).Returns<HttpContextBase>(null);

            var data = Tab.GetData(Tab.TabContextMock.Object) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Equal(RouteTable.Routes.Count, data.Count);
        }

        [Fact(Skip = "Work in progress")]
        public void ReturnRouteInstancesEvenWhenRoutesTableEmpty()
        {
            RouteTable.Routes.Clear();
            var data = Tab.GetData(Tab.TabContextMock.Object) as IList<RouteModel>;
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact(Skip = "Work in progress")]
        public void ReturnProperNumberOfInstances()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.Ignore("Something");

            var data = Tab.GetData(Tab.TabContextMock.Object) as IList<RouteModel>;

            Assert.NotNull(data);
            Assert.Equal(RouteTable.Routes.Count, data.Count);
        }

        [Fact]
        public void SubscribeToConstraintMessageTypes()
        {
            var messageBrokerMock = new Mock<IMessageBroker>();
            var setupMock = new Mock<ITabSetupContext>();
            setupMock.Setup(c => c.MessageBroker).Returns(messageBrokerMock.Object);
            Tab.Setup(setupMock.Object);

            messageBrokerMock.Verify(mb => mb.Subscribe(It.IsAny<Action<Route.ProcessConstraint.Message>>()));
        }

        [Fact(Skip = "Work in progress")]
        public void MatchConstraintMessageToRoute()
        {
            // create a TabDataStore, and configure a ITabSetupContext and ITabContext which will return it
            var store = new DictionaryDataStoreAdapter(new Dictionary<object, object>());
            var setupMock = new Mock<ITabSetupContext>();
            setupMock.Setup(s => s.GetTabStore()).Returns(store);
            Tab.TabContextMock.Setup(s => s.TabStore).Returns(store);

            // set up the test route
            var routeHandler = new Mock<IRouteHandler>();
            var constraints = new RouteValueDictionary(new { controller = "zz", action = "bb" });
            var route1 = new System.Web.Routing.Route("url", null, constraints, routeHandler.Object);
  
            // send a constraint-processed message
            var msg = new Route.ProcessConstraint.Message(new Route.ProcessConstraint.Arguments(new object[] { null, null, "controller", null, RouteDirection.IncomingRequest }), null, null, null, false);
            Routes.Persist(msg, setupMock.Object);

            // check the output
            var model1 = Tab.GetRouteModelForRoute(Tab.TabContextMock.Object, route1, null);

            Assert.Equal("url", model1.Url);
            Assert.NotNull(model1.Constraints);
            Assert.Equal(2, model1.Constraints.Count());

            var matchedConstraint = model1.Constraints.First(c => c.ParameterName == "controller");
            Assert.NotNull(matchedConstraint);
            Assert.True(matchedConstraint.Checked);
            Assert.False(matchedConstraint.Matched);

            var unmatchedConstraint = model1.Constraints.First(c => c.ParameterName != "controller");
            Assert.NotNull(unmatchedConstraint);
            Assert.False(unmatchedConstraint.Checked);
        }
    }
}