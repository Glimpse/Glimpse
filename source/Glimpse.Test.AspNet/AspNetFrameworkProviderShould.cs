using System;
using System.IO;
using System.Web;
using Glimpse.Core2;
using Glimpse.Test.AspNet.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet
{
    public class AspNetFrameworkProviderShould : IDisposable
    {
        private AspNetFrameworkProviderTester tester;

        public AspNetFrameworkProviderTester FrameworkProvider
        {
            get { return tester ?? (tester = AspNetFrameworkProviderTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            FrameworkProvider = null;
        }

        [Fact]
        public void HaveARuntimeContextTypeOfHttpContextBase()
        {
            Assert.True(FrameworkProvider.RuntimeContext.GetType().IsSubclassOf(typeof (HttpContextBase)));
        }

        [Fact]
        public void HaveARuntimeContext()
        {
            Assert.NotNull(FrameworkProvider.RuntimeContext);
            Assert.True(FrameworkProvider.RuntimeContext is HttpContextBase);
        }

        [Fact]
        public void HaveHttpRequestStore()
        {
            Assert.NotNull(FrameworkProvider.HttpRequestStore);
            Assert.Equal(5, FrameworkProvider.HttpRequestStore.Get<int>());
            Assert.Equal("TestString", FrameworkProvider.HttpRequestStore.Get<string>());
        }

        [Fact]
        public void HaveHttpServerStore()
        {
            Assert.NotNull(FrameworkProvider.HttpServerStore);
            Assert.Equal("testValue", FrameworkProvider.HttpServerStore.Get("testKey"));

            FrameworkProvider.HttpApplicationStateMock.Verify(st => st.Get("testKey"), Times.Once());
        }

 
        [Fact]
        public void SetHttpResponseHeader()
        {
            var headerName = "testKey";
            var headerValue = "testValue";

            FrameworkProvider.SetHttpResponseHeader(headerName, headerValue);

            FrameworkProvider.HttpResponseMock.Verify(r=>r.AppendHeader(headerName, headerValue));
        }

        [Fact]
        public void InjectHttpResponseBody()
        {
            var outputString = "<script src=\"test.js\"></script>";

            FrameworkProvider.InjectHttpResponseBody(outputString);

            FrameworkProvider.HttpContextMock.VerifyGet(ctx => ctx.Response);
            FrameworkProvider.HttpResponseMock.VerifyGet(r => r.Filter);
            FrameworkProvider.HttpResponseMock.VerifySet(r => r.Filter = It.IsAny<Stream>());
        }

        [Fact]
        public void SetHttpResponseStatusCode()
        {
            var statusCode = 200;
            FrameworkProvider.SetHttpResponseStatusCode(statusCode);

            FrameworkProvider.HttpResponseMock.VerifySet(r => r.StatusCode = statusCode);
            FrameworkProvider.HttpResponseMock.VerifySet(r => r.StatusDescription = null);
        }
    }
}