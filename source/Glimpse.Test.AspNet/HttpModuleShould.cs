using System;
using Glimpse.Core2.Extensibility;
using Glimpse.Test.AspNet.Tester;
using Xunit;
using Moq;

namespace Glimpse.Test.AspNet
{
    public class HttpModuleShould:IDisposable
    {
        private HttpModuleTester tester;

        private HttpModuleTester HttpModule
        {
            get { return tester ?? (tester = HttpModuleTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            HttpModule = null;
        }

        [Fact]
        public void GetGlimpseRuntimeFromAppState()
        {
            var runtime = HttpModule.GetRuntime(HttpModule.AppStateMock.Object);

            Assert.Equal(HttpModule.RuntimeMock.Object, runtime);
        }

        [Fact]
        public void CallGlimpseRuntimeBeginRequestOnBeginRequest()
        {
            HttpModule.BeginRequest(HttpModule.ContextMock.Object);

            HttpModule.RuntimeMock.Verify(r=>r.BeginRequest(), Times.Once());
        }

        [Fact]
        public void CallGlimpseExecuteTabsRequestOnPostRequestHandlerExecute()
        {
            HttpModule.PostRequestHandlerExecute(HttpModule.ContextMock.Object);

            HttpModule.RuntimeMock.Verify(r => r.ExecuteTabs(LifeCycleSupport.SessionAccessEnd), Times.Once());
        }

        [Fact]
        public void CallGlimpseExecuteTabsRequestOnPostReleaseRequestState()
        {
            HttpModule.PostReleaseRequestState(HttpModule.ContextMock.Object);

            HttpModule.RuntimeMock.Verify(r => r.ExecuteTabs(), Times.Once());
        }


        [Fact]
        public void DisposeNothing()
        {
            HttpModule.Dispose();
        }
    }
}