using System;
using System.Reflection;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class HttpModule : IHttpModule  
    {
        private readonly Factory factory = new Factory(new AspNetServiceLocator());

        public void Init(HttpApplication httpApplication)
        {
            Init(WithTestable(httpApplication));
        }

        internal void Init(HttpApplicationBase httpApplication)
        {
            var runtime = GetRuntime(httpApplication.Application);

            AppDomain.CurrentDomain.SetData(Constants.LoggerKey, factory.InstantiateLogger());

            if (runtime.IsInitialized || runtime.Initialize())
            {
                httpApplication.BeginRequest += (context, e) => BeginRequest(WithTestable(context));
                httpApplication.PostAcquireRequestState += (context, e) => BeginSessionAccess(WithTestable(context));
                httpApplication.PostRequestHandlerExecute += (context, e) => EndSessionAccess(WithTestable(context));
                httpApplication.PostReleaseRequestState += (context, e) => EndRequest(WithTestable(context));
                AppDomain.CurrentDomain.DomainUnload += UnloadDomain;
            }
        }

        internal void UnloadDomain(object sender, EventArgs e)
        {
            var appDomain = sender as AppDomain;
            var logger = appDomain.GetData(Constants.LoggerKey) as ILogger;
            string shutDownMessage = "Reason for shutdown: ";
            var httpRuntimeType = typeof(HttpRuntime);

            // Get shutdown message from HttpRuntime via ScottGu: http://weblogs.asp.net/scottgu/archive/2005/12/14/433194.aspx
            var httpRuntime = httpRuntimeType.InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null) as HttpRuntime;
            if (httpRuntime != null)
            {
                shutDownMessage += httpRuntimeType.InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, httpRuntime, null) as string;
            }
            else
            {
                shutDownMessage += "unknown.";
            }

            if (logger != null)
            {
                logger.Fatal("App domain with Id: '{0}' and BaseDirectory: '{1}' has been unloaded. Any in memory data stores have been lost.{2}", appDomain.Id, appDomain.BaseDirectory, shutDownMessage);
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

        private static HttpApplicationBase WithTestable(HttpApplication httpApplication)
        {
            return new HttpApplicationWrapper(httpApplication);
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
