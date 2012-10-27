using System.Web;
using System.Web.SessionState;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class HttpModule : IHttpModule  
    {
        public void Init(HttpApplication httpApplication)
        {
            var runtime = GetRuntime(new HttpApplicationStateWrapper(httpApplication.Application));

            if (runtime.IsInitialized || runtime.Initialize())
            {
                httpApplication.BeginRequest += (context, e) => BeginRequest(WithTestable(context));
                httpApplication.PostAcquireRequestState += (context, e) => BeginSessionAccess(WithTestable(context));
                httpApplication.PostRequestHandlerExecute += (context, e) => EndSessionAccess(WithTestable(context));
                httpApplication.PostReleaseRequestState += (context, e) => EndRequest(WithTestable(context));
            }
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        internal IGlimpseRuntime GetRuntime(HttpApplicationStateBase applicationState)
        {
            var runtime = applicationState[Constants.RuntimeKey] as IGlimpseRuntime;

            if (runtime == null)
            {
                var factory = new Factory(new AspNetServiceLocator());

                runtime = factory.InstantiateRuntime();

                applicationState.Add(Constants.RuntimeKey, runtime);
            }

            return runtime;
        }

        internal void BeginRequest(HttpContextBase httpContext)
        {
            // TODO: Add Logging to either methods here or in Runtime
            var runtime = GetRuntime(httpContext.Application);

            runtime.BeginRequest();
        }

        internal void EndRequest(HttpContextBase httpContext)
        {
            var runtime = GetRuntime(httpContext.Application);

            runtime.EndRequest();
        }

        private static HttpContextBase WithTestable(object sender)
        {
            var httpApplication = sender as HttpApplication;

            return new HttpContextWrapper(httpApplication.Context);
        }

        private void BeginSessionAccess(HttpContextBase httpContext)
        {
            var runtime = GetRuntime(httpContext.Application);

            runtime.BeginSessionAccess();
        }

        private void EndSessionAccess(HttpContextBase httpContext)
        {
            var runtime = GetRuntime(httpContext.Application);

            runtime.EndSessionAccess();
        }
    }
}
