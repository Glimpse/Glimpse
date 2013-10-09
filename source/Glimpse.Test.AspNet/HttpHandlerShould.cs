using System;
using System.Web;
using Glimpse.AspNet;
using Glimpse.Core.Framework;
using Glimpse.Test.AspNet.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet
{
    public class HttpHandlerShould:IDisposable
    {
        private HttpHandlerTester tester;
        public HttpHandlerTester Handler
        {
            get { return tester ?? (tester = HttpHandlerTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Handler = null;
        }

        [Fact]
        public void BeReusable()
        {
            Assert.True(Handler.IsReusable);
        }

        [Fact]
        public void Return404WithoutGlimpseRuntime()
        {
            Handler.ApplicationStateMock.Setup(a => a.Get(Constants.RuntimeKey)).Returns<GlimpseRuntimeWrapper>(null);

            Assert.Throws<HttpException>(()=>Handler.ProcessRequest(Handler.ContextMock.Object));
        }

        [Fact]
        public void RunResourceWithNameMatch()
        {
            Handler.ProcessRequest(Handler.ContextMock.Object);

            Handler.RuntimeMock.Verify(r=>r.ExecuteResource(Handler.ResourceName, It.IsAny<ResourceParameters>()),Times.Once());
        }

        [Fact]
        public void RunDefaultResourceWithoutNameMatch()
        {
            Handler.QueryString.Clear();

            Handler.ProcessRequest(Handler.ContextMock.Object);

            Handler.RuntimeMock.Verify(r => r.ExecuteDefaultResource(), Times.Once());
        }
    }
}