using System;
using System.Reflection;
using Glimpse.AspNet;
using Glimpse.Core.Framework;
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

            HttpModule.RuntimeMock.Verify(r=>r.BeginRequest(It.IsAny<IFrameworkProvider>()), Times.Once());
        }

        [Fact]
        public void DisposeNothing()
        {
            HttpModule.Dispose();
        }

        [Fact]
        public void HaveLoggedAppDomainUnloadMessage()
        {
            // to make sure the HttpModule's type constructor has been run, otherwise the previous logger will have a value of null, which will be 
            // restored after the test, making other tests fail, because the HttpModule's type constructor will eventually be run by calling the 
            // OnAppDomainUnload method, which will set the correct logger, but that will be undone by setting the null value back.
            Assert.NotNull(this.HttpModule);

            var currentDomain = AppDomain.CurrentDomain;

            object previousLoggerKeyValue = currentDomain.GetData(Constants.LoggerKey);
            try
            {
                currentDomain.SetData(Constants.LoggerKey, HttpModule.LoggerMock.Object);
                typeof(HttpModule).GetMethod("OnAppDomainUnload", BindingFlags.NonPublic | BindingFlags.Static).Invoke(HttpModule, new object[] { currentDomain });
                HttpModule.LoggerMock.Verify(l => l.Fatal(It.IsAny<string>(), It.IsAny<object[]>()));
            }
            finally
            {
                currentDomain.SetData(Constants.LoggerKey, previousLoggerKeyValue);
            }
        }

        [Fact]
        public void HaveStoredLoggerInAppDomainData()
        {
            Assert.NotNull(this.HttpModule); // triggering the call of the HttpModule's type constructor (if not already called)
            Assert.NotNull(AppDomain.CurrentDomain.GetData(Constants.LoggerKey));
        }
    }
}