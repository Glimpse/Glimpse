using System.Collections.Generic;
using System.Web;
using Glimpse.AspNet;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet
{
    public class AspNetRuntimeServiceShould
    {
        [Fact]
        public void HaveARuntimeContextTypeOfHttpContextBase()
        {
            var service = new AspNetRuntimeService();

            Assert.Equal(typeof (HttpContextBase), service.RuntimeContextType);
        }

        [Fact]
        public void HaveARuntimeContext()
        {
            var contextMock = new Mock<HttpContextBase>();
            var service = new AspNetRuntimeService {Context = contextMock.Object};

            Assert.NotNull(service.RuntimeContext);
            Assert.True(service.RuntimeContext is HttpContextBase);
        }

        [Fact]
        public void HaveMatchingContextAndContextTypes()
        {
            var contextMock = new Mock<HttpContextBase>();
            var service = new AspNetRuntimeService {Context = contextMock.Object};

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
                                              {typeof (string), "TestString"}, 
                                              {typeof (int), 5}
                                          });
                                      

            var service = new AspNetRuntimeService {Context = contextMock.Object};

            Assert.NotNull(service.HttpRequestStore);
            Assert.Equal(5, service.HttpRequestStore.Get<int>());
            Assert.Equal("TestString", service.HttpRequestStore.Get<string>());
        }

        [Fact]
        public void HaveHttpServerStore()
        {
            Assert.False(true);
        }
    }
}