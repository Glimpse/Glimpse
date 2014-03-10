using System;
using System.IO;
using System.Web;
using Glimpse.Core.Extensions;
using Glimpse.Test.AspNet.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet
{
    public class AspNetFrameworkProviderShould : IDisposable
    {
        private AspNetRequestResponseAdapterTester tester;

        public AspNetRequestResponseAdapterTester RequestResponseAdapter
        {
            get { return tester ?? (tester = AspNetRequestResponseAdapterTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            RequestResponseAdapter = null;
        }

        [Fact]
        public void HaveARuntimeContextTypeOfHttpContextBase()
        {
            Assert.True(RequestResponseAdapter.RuntimeContext.GetType().IsSubclassOf(typeof (HttpContextBase)));
        }

        [Fact]
        public void HaveARuntimeContext()
        {
            Assert.NotNull(RequestResponseAdapter.RuntimeContext);
            Assert.True(RequestResponseAdapter.RuntimeContext is HttpContextBase);
        }

        [Fact]
        public void SetHttpResponseHeader()
        {
            var headerName = "testKey";
            var headerValue = "testValue";

            RequestResponseAdapter.SetHttpResponseHeader(headerName, headerValue);

            RequestResponseAdapter.HttpResponseMock.Verify(r=>r.AppendHeader(headerName, headerValue));
        }

        [Fact]
        public void InjectHttpResponseBody()
        {
            var outputString = "<script src=\"test.js\"></script>";

            RequestResponseAdapter.InjectHttpResponseBody(outputString);

            RequestResponseAdapter.HttpContextMock.VerifyGet(ctx => ctx.Response);
            RequestResponseAdapter.HttpResponseMock.VerifyGet(r => r.Filter);
            RequestResponseAdapter.HttpResponseMock.VerifySet(r => r.Filter = It.IsAny<Stream>());
        }

        [Fact]
        public void SetHttpResponseStatusCode()
        {
            var statusCode = 200;
            RequestResponseAdapter.SetHttpResponseStatusCode(statusCode);

            RequestResponseAdapter.HttpResponseMock.VerifySet(r => r.StatusCode = statusCode);
            RequestResponseAdapter.HttpResponseMock.VerifySet(r => r.StatusDescription = null);
        }
    }
}