using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Tab;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.Tab
{
    public class ServerShould
    {
        [Fact]
        public void HaveProperContextObjectType()
        {
            var server = new Server();

            Assert.Equal(typeof(HttpContextBase), server.RequestContextType);
        }

        [Fact]
        public void UseDefaultLifeCycleSupport()
        {
            var server = new Server();
            Assert.Equal(RuntimeEvent.EndRequest, server.ExecuteOn);
        }

        [Fact]
        public void BeNamedRequest()
        {
            var server = new Server();
            Assert.Equal("Server", server.Name);
        }

        [Fact]
        public void HaveADocumentationUri()
        {
            var server = new Server();

            Assert.False(string.IsNullOrWhiteSpace(server.DocumentationUri));
        }

        [Fact]
        public void ReturnData()
        {
            var serverVariables = new NameValueCollection { { "Name", "Value" }, { "Name1", "Value1" } };
             
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(r => r.ServerVariables).Returns(serverVariables);
            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.Request).Returns(requestMock.Object); 
            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

            var server = new Server();
            var result = server.GetData(contextMock.Object);

            Assert.NotNull(result);

            var dictionaryResult = result as Dictionary<string, string>;
            Assert.NotNull(dictionaryResult);
            Assert.NotNull(dictionaryResult.Count);
        }
    }
}
