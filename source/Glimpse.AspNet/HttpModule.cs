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
        private static readonly Factory Factory;

        static HttpModule()
        {
            var serviceLocator = new AspNetServiceLocator();
            Factory = new Factory(serviceLocator);
            ILogger logger = Factory.InstantiateLogger();
            serviceLocator.Logger = logger;

            try
            {
                BuildManager.GetReferencedAssemblies();
                serviceLocator.Logger.Debug("Preloaded all referenced assemblies with System.Web.Compilation.BuildManager.GetReferencedAssemblies()");
            }
            catch (Exception exception)
            {
                serviceLocator.Logger.Error("Call to System.Web.Compilation.BuildManager.GetReferencedAssemblies() failed.", exception);
            }

            AppDomain.CurrentDomain.SetData(Constants.LoggerKey, logger);
            AppDomain.CurrentDomain.DomainUnload += (sender, e) => OnAppDomainUnload((AppDomain)sender);
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
            var runtime = GetRuntime(httpApplication.Application);

            if (runtime.IsInitialized || runtime.Initialize())
            {
                httpApplication.BeginRequest += (context, e) => OnBeginRequest(WithTestable(context));
                httpApplication.PostAcquireRequestState += (context, e) => OnPostAcquireRequestState(WithTestable(context));
                httpApplication.PostRequestHandlerExecute += (context, e) => OnPostRequestHandlerExecute(WithTestable(context));
                httpApplication.PostReleaseRequestState += (context, e) => OnPostReleaseRequestState(WithTestable(context));
                httpApplication.EndRequest += (context, e) => OnEndRequest(WithTestable(context));
                httpApplication.PreSendRequestHeaders += (context, e) => OnPreSendRequestHeaders(WithTestable(context));
            }
        }

        internal IGlimpseRuntime GetRuntime(HttpApplicationStateBase applicationState)
        {
            var runtime = applicationState[Constants.RuntimeKey] as IGlimpseRuntime;

            if (runtime == null)
            {
                lock (LockObj)
                {
                    runtime = applicationState[Constants.RuntimeKey] as IGlimpseRuntime;

                    if (runtime == null)
                    {
                        runtime = Factory.InstantiateRuntime();

                        applicationState.Add(Constants.RuntimeKey, runtime);
                    }
                }
            }

            return runtime;
        }

        internal void OnBeginRequest(HttpContextBase httpContext)
        {
            // TODO: Add Logging to either methods here or in Runtime
            var runtime = GetRuntime(httpContext.Application);

            runtime.BeginRequest();
        }

        internal void OnEndRequest(HttpContextBase httpContext)
        {
            if(!httpContext.Items.Contains("EndRequestCalled"))
            {
                this.OnPostReleaseRequestState(httpContext);
            }
        }

        internal void OnPostReleaseRequestState(HttpContextBase httpContext)
        {
            httpContext.Items.Add("EndRequestCalled", true);
            
            var runtime = GetRuntime(httpContext.Application);

            runtime.EndRequest();
        }

        internal void OnPreSendRequestHeaders(HttpContextBase httpContext)
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

        private void OnPostAcquireRequestState(HttpContextBase httpContext)
        {
            var runtime = GetRuntime(httpContext.Application);

            runtime.BeginSessionAccess();
        }

        private void OnPostRequestHandlerExecute(HttpContextBase httpContext)
        {
            var runtime = GetRuntime(httpContext.Application);

            runtime.EndSessionAccess();
        }
    }
}
