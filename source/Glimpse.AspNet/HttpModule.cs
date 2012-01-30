using System.Web;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.AspNet
{
    public class HttpModule:IHttpModule
    {

        public void Init(HttpApplication httpApplication)
        {
            var runtime = GetRuntime(new HttpApplicationStateWrapper(httpApplication.Application));

            if (runtime.IsInitialized || runtime.Initialize())
            {
                httpApplication.BeginRequest += (context, e) => BeginRequest(WithTestable(context));
                httpApplication.PostRequestHandlerExecute += (context, e) => PostRequestHandlerExecute(WithTestable(context));
                httpApplication.PostReleaseRequestState += (context, e) => PostReleaseRequestState(WithTestable(context));
                httpApplication.EndRequest += (context, e) => EndRequest(WithTestable(context));
            }
        }

        internal IGlimpseRuntime GetRuntime(HttpApplicationStateBase applicationState)
        {
            var runtime = applicationState[Constants.RuntimeKey] as IGlimpseRuntime;

            if (runtime == null)
            {
                var config = applicationState[Constants.ConfigKey] as GlimpseConfiguration ?? new GlimpseConfiguration(new AspNetFrameworkProvider(), new HttpHandlerEndpointConfiguration());

                runtime = new GlimpseRuntime(config);

                applicationState.Add(Constants.RuntimeKey, runtime);
            }

            return runtime;
        }

        private static HttpContextBase WithTestable(object sender)
        {
            var httpApplication = sender as HttpApplication;

            return new HttpContextWrapper(httpApplication.Context);
        }

        internal void BeginRequest(HttpContextBase httpContext)
        {
            //TODO: Add Logging to either methods here or in Runtime
            var runtime = GetRuntime(httpContext.Application);

            runtime.BeginRequest();
        }

        //TODO: Figure out the proper SessionAccessEnd ASP.NET event
        internal void PostRequestHandlerExecute(HttpContextBase httpContext)
        {
            var runtime = GetRuntime(httpContext.Application);

            runtime.ExecuteTabs(LifeCycleSupport.SessionAccessEnd);
        }

        internal void PostReleaseRequestState(HttpContextBase httpContext)
        {
            var runtime = GetRuntime(httpContext.Application);

            runtime.ExecuteTabs();
        }

        internal void EndRequest(HttpContextBase httpContext)
        {
            var runtime = GetRuntime(httpContext.Application);

            runtime.EndRequest();
        }


        public void Dispose()
        {
            //Nothing to dispose
        }
    }
}
