using System;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class HttpModule : IHttpModule  
    {
        private static readonly object LockObj = new object();
        private static GlimpseConfiguration Configuration;

        static HttpModule()
        {
            // V2Merge: need to find another way to access logger here
            // ILogger logger = Factory.InstantiateLogger();
            // serviceLocator.Logger = Factory.InstantiateLogger();

            try
            {
                BuildManager.GetReferencedAssemblies();
                // TODO: Add these back in
                // serviceLocator.Logger.Debug("Preloaded all referenced assemblies with System.Web.Compilation.BuildManager.GetReferencedAssemblies()");
            }
            catch (Exception exception)
            {
                // TODO: Add these back in
                // serviceLocator.Logger.Error("Call to System.Web.Compilation.BuildManager.GetReferencedAssemblies() failed.", exception);
            }

            // AppDomain.CurrentDomain.SetData(Constants.LoggerKey, logger);
            // AppDomain.CurrentDomain.DomainUnload += (sender, e) => OnAppDomainUnload((AppDomain)sender);
        }

        private static void OnAppDomainUnload(AppDomain appDomain)
        {
            ILogger logger = appDomain.GetData(Constants.LoggerKey) as ILogger;

            if (logger == null)
            {
                return;
            }

            logger.Fatal(
                "AppDomain with Id: '{0}' and BaseDirectory: '{1}' has been unloaded. Any in memory data stores have been lost. {2}",
                appDomain.Id,
                appDomain.BaseDirectory,
                HttpRuntimeShutdownMessageResolver.ResolveShutdownMessage());

            // NLog writes its logs asynchronously, which means that if we don't wait, chances are the log will not be written 
            // before the appdomain is actually shut down, so we sleep for 100ms and hopefully that is enough for NLog to do its thing
            Thread.Sleep(100);
        }

        public void Init(HttpApplication httpApplication)
        {
            Init(WithTestable(httpApplication));
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        internal void Init(HttpApplicationBase httpApplication)
        {
            if (!GlimpseRuntime.IsInitialized)
            {
                Configuration = Configuration ?? 
                    new GlimpseConfiguration(
                        new HttpHandlerEndpointConfiguration(), 
                        new InMemoryPersistenceStore(
                            new HttpApplicationStateBaseDataStoreAdapter(httpApplication.Application)));

                GlimpseRuntime.Initialize(Configuration);
            }

            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.SetData(Constants.LoggerKey, Configuration.Logger);
            currentDomain.DomainUnload += (sender, e) => OnAppDomainUnload((AppDomain)sender);

            httpApplication.BeginRequest += (context, e) => BeginRequest(WithTestable(context));
            httpApplication.PostAcquireRequestState += (context, e) => BeginSessionAccess(WithTestable(context));
            httpApplication.PostRequestHandlerExecute += (context, e) => EndSessionAccess(WithTestable(context));
            httpApplication.PostReleaseRequestState += (context, e) => EndRequest(WithTestable(context));
            httpApplication.PreSendRequestHeaders += (context, e) => SendHeaders(WithTestable(context));
        }

        internal void BeginRequest(HttpContextBase httpContext)
        {
            // TODO: Add Logging to either methods here or in Runtime

            GlimpseRuntime.Instance.BeginRequest(new AspNetRequestResponseAdapter(httpContext, Configuration.Logger));
        }

        internal void EndRequest(HttpContextBase httpContext)
        {
            GlimpseRuntime.Instance.EndRequest(new AspNetRequestResponseAdapter(httpContext, Configuration.Logger));
        }

        internal void SendHeaders(HttpContextBase httpContext)
        {
            httpContext.HeadersSent(true);
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
            GlimpseRuntime.Instance.BeginSessionAccess(new AspNetRequestResponseAdapter(httpContext, Configuration.Logger));
        }

        private void EndSessionAccess(HttpContextBase httpContext)
        {
            GlimpseRuntime.Instance.EndSessionAccess(new AspNetRequestResponseAdapter(httpContext, Configuration.Logger));
        }
    }
}
