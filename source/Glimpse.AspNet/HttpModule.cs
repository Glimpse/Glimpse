using System.Web;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.AspNet
{
    public class HttpModule:IHttpModule
    {
        private const string RuntimeKey = "__GlimpseRuntime";

        public void Init(HttpApplication httpApplication)
        {
            //TODO: Check to see if runtime exists - think about difference between WebForms and MVC
            var configuration = new GlimpseConfiguration(new AspNetFrameworkProvider(), new HttpHandlerEndpointConfiguration());
            var runtime = new GlimpseRuntime(configuration);

            var appState = httpApplication.Application;

            if (appState[RuntimeKey] == null)
                    appState.Add(RuntimeKey, runtime);

            runtime.Initialize();

            httpApplication.BeginRequest += BeginRequest;
            httpApplication.PostRequestHandlerExecute += ExecutePluginsWithSessionState;
            httpApplication.PostReleaseRequestState += ExecutePluginsWithoutSessionState;
            httpApplication.EndRequest += EndRequest;
        }

        internal void BeginRequest(object sender, System.EventArgs e)
        {
            var runtime = TryGetGlimpseRuntime(sender);

            runtime.BeginRequest();
        }

        internal void ExecutePluginsWithSessionState(object sender, System.EventArgs e)
        {
            var runtime = TryGetGlimpseRuntime(sender);

            runtime.ExecuteTabs(LifeCycleSupport.SessionAccessBegin);
        }

        internal void ExecutePluginsWithoutSessionState(object sender, System.EventArgs e)
        {
            var runtime = TryGetGlimpseRuntime(sender);

            runtime.ExecuteTabs();
        }

        internal void EndRequest(object sender, System.EventArgs e)
        {
            var runtime = TryGetGlimpseRuntime(sender);

            runtime.EndRequest();
        }


        public void Dispose()
        {
            //Nothing to dispose
        }

        private GlimpseRuntime TryGetGlimpseRuntime(object sender)
        {
            var httpApplication = sender as HttpApplication;

            if (httpApplication == null) return null;

            return httpApplication.Application[RuntimeKey] as GlimpseRuntime;
        }

    }
}
