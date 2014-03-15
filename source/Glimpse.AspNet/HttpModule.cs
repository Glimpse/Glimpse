using System;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class HttpModule : IHttpModule
    {
        private static Configuration Configuration;

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

            AppDomain.CurrentDomain.DomainUnload += (sender, e) => OnAppDomainUnload((AppDomain)sender);
        }

        private static void OnAppDomainUnload(AppDomain appDomain)
        {
            if (GlimpseRuntime.IsInitialized)
            {
                GlimpseRuntime.Instance.Configuration.Logger.Fatal(
                    "AppDomain with Id: '{0}' and BaseDirectory: '{1}' has been unloaded. Any in memory data stores have been lost. {2}",
                    appDomain.Id,
                    appDomain.BaseDirectory,
                    "Reason for shutdown => " + HostingEnvironment.ShutdownReason);

                // NLog writes its logs asynchronously, which means that if we don't wait, chances are the log will not be written 
                // before the appdomain is actually shut down, so we sleep for 100ms and hopefully that is enough for NLog to do its thing
                Thread.Sleep(100);
            }
        }

        public void Init(HttpApplication httpApplication)
        {
            Init(new HttpApplicationWrapper(httpApplication));
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
                    new Configuration(
                        new HttpHandlerEndpointConfiguration(),
                        new InMemoryPersistenceStore(new HttpApplicationStateBaseDataStoreAdapter(httpApplication.Application)),
                        new AspNetCurrentGlimpseRequestIdTracker());

                GlimpseRuntime.Initialize(Configuration);
            }

            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.SetData(Constants.LoggerKey, Configuration.Logger);

            Func<object, HttpContextWrapper> createHttpContextWrapper = sender => new HttpContextWrapper(((HttpApplication)sender).Context);

            httpApplication.BeginRequest += (context, e) => BeginRequest(createHttpContextWrapper(context));
            httpApplication.PostAcquireRequestState += (context, e) => BeginSessionAccess(createHttpContextWrapper(context));
            httpApplication.PostRequestHandlerExecute += (context, e) => EndSessionAccess(createHttpContextWrapper(context));
            httpApplication.PostReleaseRequestState += (context, e) => EndRequest(createHttpContextWrapper(context));
            httpApplication.PreSendRequestHeaders += (context, e) => SendHeaders(createHttpContextWrapper(context));
        }

        internal void BeginRequest(HttpContextBase httpContext)
        {
            // TODO: Add Logging to either methods here or in Runtime

            var glimpseRequestContextHandle = GlimpseRuntime.Instance.BeginRequest(new AspNetRequestResponseAdapter(httpContext, Configuration.Logger));
            if (glimpseRequestContextHandle.RequestHandlingMode != RequestHandlingMode.Unhandled)
            {
                // We'll store the glimpseRequestContextHandle in the Items collection so it can be retrieved and disposed later on in the EndRequest event handler.
                // If for some reason EndRequest would not be called for this request, then the Items collection will still be cleaned up by the ASP.NET
                // runtime and the glimpseRequestContextHandle will then loose its last reference and will eventually be finalized, which will dispose the handle anyway.
                httpContext.Items.Add(Constants.GlimpseRequestContextHandle, glimpseRequestContextHandle);
            }
        }

        private static void BeginSessionAccess(HttpContextBase httpContext)
        {
            ProcessAspNetRuntimeEvent(httpContext, GlimpseRuntime.Instance.BeginSessionAccess);
        }

        private static void EndSessionAccess(HttpContextBase httpContext)
        {
            ProcessAspNetRuntimeEvent(httpContext, GlimpseRuntime.Instance.EndSessionAccess);
        }

        private static void EndRequest(HttpContextBase httpContext)
        {
            ProcessAspNetRuntimeEvent(httpContext, GlimpseRuntime.Instance.EndRequest, true);
        }

        private static void SendHeaders(HttpContextBase httpContext)
        {
            ProcessAspNetRuntimeEvent(httpContext, glimpseRequestContextHandle =>
            {
                IGlimpseRequestContext glimpseRequestContext;
                if (GlimpseRuntime.Instance.TryGetRequestContext(glimpseRequestContextHandle.GlimpseRequestId, out glimpseRequestContext))
                {
                    ((IAspNetRequestResponseAdapter)glimpseRequestContext.RequestResponseAdapter).PreventSettingHttpResponseHeaders();
                }
            }, availabilityOfGlimpseRequestContextHandleIsRequired: false);

            // Under normal circumstances the SendHeaders event will be raised AFTER the EndRequest
            // event, which means that in most cases the Glimpse request context handle will already
            // be disposed as expected. It is only when there are premature flushes (before
            // EndRequest) that the Glimpse request context handle will be found. The
            // PreSendRequestHeaders event is raised non deterministic by default, see
            // http://support.microsoft.com/kb/307985/en-us (although article dates from NET 1.1,
            // tests confirmed it's still applicable), that is why we set the
            // availabilityOfGlimpseRequestContextHandleIsRequired = false
        }

        private static void ProcessAspNetRuntimeEvent(HttpContextBase httpContext, Action<GlimpseRequestContextHandle> action, bool disposeHandle = false, bool availabilityOfGlimpseRequestContextHandleIsRequired = true)
        {
            if (GlimpseRuntime.IsInitialized)
            {
                GlimpseRequestContextHandle glimpseRequestContextHandle;
                if (TryGetGlimpseRequestContextHandle(httpContext, out glimpseRequestContextHandle))
                {
                    try
                    {
                        action(glimpseRequestContextHandle);
                    }
                    finally
                    {
                        if (disposeHandle)
                        {
                            glimpseRequestContextHandle.Dispose();
                            httpContext.Items.Remove(Constants.GlimpseRequestContextHandle);
                        }
                    }
                }
                else if (availabilityOfGlimpseRequestContextHandleIsRequired)
                {
                    Configuration.Logger.Info("There is no Glimpse request context handle stored inside the httpContext.Items collection.");
                }
            }
        }

        private static bool TryGetGlimpseRequestContextHandle(HttpContextBase httpContext, out GlimpseRequestContextHandle glimpseRequestContextHandle)
        {
            glimpseRequestContextHandle = null;

            if (httpContext.Items.Contains(Constants.GlimpseRequestContextHandle))
            {
                glimpseRequestContextHandle = (GlimpseRequestContextHandle)httpContext.Items[Constants.GlimpseRequestContextHandle];
                if (glimpseRequestContextHandle != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}