using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Glimpse.AspNet;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet
{
    public class AspNetFrameworkProviderShould
    {
        [Fact]
        public void HaveARuntimeContextTypeOfHttpContextBase()
        {
            var service = new AspNetFrameworkProvider();

            Assert.Equal(typeof (HttpContextBase), service.RuntimeContextType);
        }

        [Fact]
        public void HaveARuntimeContext()
        {
            var contextMock = new Mock<HttpContextBase>();
            var service = new AspNetFrameworkProvider {Context = contextMock.Object};

            Assert.NotNull(service.RuntimeContext);
            Assert.True(service.RuntimeContext is HttpContextBase);
        }

        [Fact]
        public void HaveMatchingContextAndContextTypes()
        {
            var contextMock = new Mock<HttpContextBase>();
            var service = new AspNetFrameworkProvider {Context = contextMock.Object};

            Assert.True(service.RuntimeContext.GetType().IsSubclassOf(service.RuntimeContextType));
            Assert.True(service.RuntimeContextType.IsInstanceOfType(service.RuntimeContext));
        }

        [Fact]
        public void HaveHttpRequestStore()
        {
            var contextMock = new Mock<HttpContextBase>();
            contextMock.Setup(ctx => ctx.Items)
                .Returns(new Dictionary<object, object>
                                          {
                                              {typeof (string).FullName, "TestString"}, 
                                              {typeof (int).FullName, 5}
                                          });
                                      

            var service = new AspNetFrameworkProvider {Context = contextMock.Object};

            Assert.NotNull(service.HttpRequestStore);
            Assert.Equal(5, service.HttpRequestStore.Get<int>());
            Assert.Equal("TestString", service.HttpRequestStore.Get<string>());
        }

        [Fact]
        public void HaveHttpServerStore()
        {
            var contextMock = new Mock<HttpContextBase>();
            var applicationStateMock = new Mock<HttpApplicationStateBase>();
            applicationStateMock.Setup(st => st.Get("testKey")).Returns("testValue");
            contextMock.Setup(ctx => ctx.Application).Returns(applicationStateMock.Object);

            var provider = new AspNetFrameworkProvider {Context = contextMock.Object};

            Assert.NotNull(provider.HttpServerStore);
            Assert.Equal("testValue", provider.HttpServerStore.Get("testKey"));
            
            applicationStateMock.Verify(st=>st.Get("testKey"), Times.Once());
        }

        [Fact(Skip = "Finish me!")]
        public void HaveRequestMetadata()
        {
            
        }

        [Fact]
        public void SetHttpResponseHeader()
        {
            var contextMock = new Mock<HttpContextBase>();
            
            var responseMock = new Mock<HttpResponseBase>();
            var headerCollection = new NameValueCollection();
            responseMock.Setup(r => r.Headers).Returns(headerCollection);

            var applicationStateMock = new Mock<HttpApplicationStateBase>();
            applicationStateMock.Setup(st => st.Get("testKey")).Returns("testValue");
            
            contextMock.Setup(ctx => ctx.Application).Returns(applicationStateMock.Object);
            contextMock.Setup(ctx => ctx.Response).Returns(responseMock.Object);

            var provider = new AspNetFrameworkProvider { Context = contextMock.Object };

            var headerName = "testKey";
            var headerValue = "testValue";

            provider.SetHttpResponseHeader(headerName, headerValue);

            contextMock.VerifyGet(ctx=>ctx.Response);
            responseMock.VerifyGet(r=>r.Headers);
            Assert.Equal(headerValue, headerCollection[headerName]);
        }
    }
}