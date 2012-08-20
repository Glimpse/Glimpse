using System;
using System.Web;
using System.Web.Routing;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Tab;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;
using System.Collections.Generic;

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

        [Fact]
        public void ReturnNullWhenRequestContextIsNull()
        {
            Tab.TabContextMock.Setup(tc => tc.GetRequestContext<HttpContextBase>()).Returns<HttpContextBase>(null);

            Assert.Null(Tab.GetData(Tab.TabContextMock.Object));
        }

        [Fact]
        public void ReturnRouteInstances()
        {
            RouteTable.Routes.Clear();
            var data = Tab.GetData(Tab.TabContextMock.Object) as IList<RouteInstance>;
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact]
        public void ReturnProperNumberOfInstances()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.Ignore("Something");

            var data = Tab.GetData(Tab.TabContextMock.Object) as IList<RouteInstance>;

            Assert.NotNull(data);
            Assert.Equal(RouteTable.Routes.Count, data.Count);
        }
    }
}