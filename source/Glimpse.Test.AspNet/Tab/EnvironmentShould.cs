using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Tab;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class EnvironmentShould
    {
        [Fact]
        public void HaveProperContextObjectType()
        {
            var request = new Glimpse.AspNet.Tab.Environment();

            Assert.Equal(typeof(HttpContextBase), request.RequestContextType);
        }

        [Fact]
        public void UseDefaultLifeCycleSupport()
        {
            var request = new Glimpse.AspNet.Tab.Environment();
            Assert.Equal(RuntimeEvent.EndRequest, request.ExecuteOn);
        }

        [Fact]
        public void BeNamedEnvironment()
        {
            var request = new Glimpse.AspNet.Tab.Environment();
            Assert.Equal("Environment", request.Name);
        }

        [Fact]
        public void HaveADocumentationUri()
        {
            var request = new Glimpse.AspNet.Tab.Environment();

            Assert.False(string.IsNullOrWhiteSpace(request.DocumentationUri));
        }

        [Fact]
        public void ReturnData()
        {
            var serverKeys = new NameValueCollection();

            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(x => x.ServerVariables).Returns(serverKeys);

            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.IsDebuggingEnabled).Returns(true);
            httpBaseMock.Setup(c => c.Request).Returns(requestMock.Object);
            httpBaseMock.Setup(c => c.Application["Glimpse.AspNet.Environment"]).Returns(null);

            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

            var request = new TestEnvironment();
            var result = request.GetData(contextMock.Object);

            Assert.NotNull(result);
            Assert.NotNull(result as EnvironmentModel);
        }

        [Fact]
        public void ReturnStoredData()
        {
            var model = new EnvironmentModel();

            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.Application["Glimpse.AspNet.Environment"]).Returns(model);

            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

            var request = new Glimpse.AspNet.Tab.Environment();
            var result = request.GetData(contextMock.Object);
             
            Assert.Same(model, result);
        }

        public class TestEnvironment : Glimpse.AspNet.Tab.Environment
        {
            protected override IEnumerable<Assembly> FindAllAssemblies()
            {
                return new List<Assembly> { Assembly.GetExecutingAssembly() };
            }
        }
    }
}
