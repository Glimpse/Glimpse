using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Tab;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class SessionShould
    {
        [Fact]
        public void HaveProperContextObjectType()
        {
            var request = new Session();

            Assert.Equal(typeof(HttpContextBase), request.RequestContextType);
        }

        [Fact]
        public void UseDefaultLifeCycleSupport()
        {
            var request = new Session();
            Assert.Equal(RuntimeEvent.EndSessionAccess, request.ExecuteOn);
        }

        [Fact]
        public void BeNamedSession()
        {
            var request = new Session();
            Assert.Equal("Session", request.Name);
        }

        [Fact]
        public void HaveADocumentationUri()
        {
            var request = new Session();

            Assert.False(string.IsNullOrWhiteSpace(request.DocumentationUri));
        }

        [Fact]
        public void ReturnData()
        {
            var sessionKeys = new NameValueCollection { { "Hello", "Fellow" }, { "World", "Jorld" } };

            var sesionMock = new Mock<HttpSessionStateBase>();
            sesionMock.Setup(r => r.Keys).Returns(sessionKeys.Keys);
            sesionMock.SetupGet(r => r.Count).Returns(2);
            sesionMock.Setup(r => r["Hello"]).Returns("Fellow");
            sesionMock.Setup(r => r["World"]).Returns("Jorld");

            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.Session).Returns(sesionMock.Object);

            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

            var request = new Session();
            var result = request.GetData(contextMock.Object);

            Assert.NotNull(result);
            Assert.NotNull(result as List<SessionModel>);
        }

        [Fact]
        public void ReturnNoData()
        { 
            var httpBaseMock = new Mock<HttpContextBase>();
            httpBaseMock.Setup(c => c.Session).Returns((HttpSessionStateBase)null);

            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.GetRequestContext<HttpContextBase>()).Returns(httpBaseMock.Object);

            var request = new Session();
            var result = request.GetData(contextMock.Object);

            Assert.Null(result); 
        }
    }
}
