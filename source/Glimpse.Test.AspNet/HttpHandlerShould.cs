using System;
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

        [Fact(Skip = "Fix to work with new init model.")]
        public void RunResourceWithNameMatch()
        {
            Handler.ProcessRequest(Handler.ContextMock.Object);

            Handler.RuntimeMock.Verify(r => r.ExecuteResource(It.IsAny<GlimpseRequestContextHandle>(), Handler.ResourceName, It.IsAny<ResourceParameters>()), Times.Once());
        }

        [Fact(Skip = "Fix to work with new init model.")]
        public void RunDefaultResourceWithoutNameMatch()
        {
            Handler.QueryString.Clear();

            Handler.ProcessRequest(Handler.ContextMock.Object);

            Handler.RuntimeMock.Verify(r => r.ExecuteDefaultResource(It.IsAny<GlimpseRequestContextHandle>()), Times.Once());
        }
    }
}