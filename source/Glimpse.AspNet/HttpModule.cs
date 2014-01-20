using System;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class HttpModule : IHttpModule
    {
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
                    new GlimpseConfiguration(
                        new HttpHandlerEndpointConfiguration(),
                        new InMemoryPersistenceStore(
                            new HttpApplicationStateBaseDataStoreAdapter(httpApplication.Application)));

                GlimpseRuntime.Initialize(Configuration);
            }

            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.SetData(Constants.LoggerKey, Configuration.Logger);
            currentDomain.DomainUnload += (sender, e) => OnAppDomainUnload((AppDomain)sender);

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

#warning as in the Owin Middleware, we want to avoid the BeginRequest to being called if we are going to execute a resource, in v1 BeginRequest always executed because the check for the RuntimePolicy.Off flag would fail
#warning it would be better to have this part of the BeginRequest which would return the handle or something containing a possible handling and indication whether or not we are dealing with a resource request, so that the logic can be shared among framework providers
            if (!httpContext.Request.RawUrl.StartsWith(Configuration.EndpointBaseUri, StringComparison.InvariantCultureIgnoreCase))
            {
                var glimpseRequestContextHandle = GlimpseRuntime.Instance.BeginRequest(new AspNetRequestResponseAdapter(httpContext, Configuration.Logger));

                // We'll store the glimpseRequestContextHandle in the Items collection so it can be retrieved and disposed later on in the EndRequest event handler.
                // If for some reason EndRequest would not be called for this request, then the Items collection will still be cleaned up by the ASP.NET
                // runtime and the glimpseRequestContextHandle will then loose its last reference and will eventually be finalized, which will dispose the handle anyway.
                httpContext.Items.Add(Constants.GlimpseRequestContextHandle, glimpseRequestContextHandle);
            }
        }

        private static void BeginSessionAccess(HttpContextBase httpContext)
        {
            ProcessAspNetRuntimeEvent("BeginSessionAccess", httpContext, GlimpseRuntime.Instance.BeginSessionAccess);
        }

        private static void EndSessionAccess(HttpContextBase httpContext)
        {
            ProcessAspNetRuntimeEvent("EndSessionAccess", httpContext, GlimpseRuntime.Instance.EndSessionAccess);
        }

        private static void EndRequest(HttpContextBase httpContext)
        {
            ProcessAspNetRuntimeEvent("EndRequest", httpContext, GlimpseRuntime.Instance.EndRequest, true);
        }

        private static void SendHeaders(HttpContextBase httpContext)
        {
            ProcessAspNetRuntimeEvent("SendHeaders", httpContext, aspNetRequestResponseAdapter => aspNetRequestResponseAdapter.PreventSettingHttpResponseHeaders());
        }

        private static void ProcessAspNetRuntimeEvent(
            string runtimeEvent,
            HttpContextBase httpContext,
            Action<IAspNetRequestResponseAdapter> action,
            bool disposeHandle = false)
        {
            if (GlimpseRuntime.IsInitialized)
            {
                try
                {
                    IAspNetRequestResponseAdapter aspNetRequestResponseAdapter;
                    if (TryGetAspNetRequestResponseAdapter(httpContext, out aspNetRequestResponseAdapter))
                    {
                        action(aspNetRequestResponseAdapter);
                    }
                    else
                    {
                        Configuration.Logger.Debug("Skipped handling of ASP.NET runtime event '" + runtimeEvent + "' due to missing request response adapter");
                    }
                }
                finally
                {
                    GlimpseRequestContextHandle glimpseRequestContextHandle;
                    if (disposeHandle && TryGetGlimpseRequestContextHandle(httpContext, out glimpseRequestContextHandle))
                    {
                        try
                        {
                            glimpseRequestContextHandle.Dispose();
                            httpContext.Items.Remove(Constants.GlimpseRequestContextHandle);
                        }
                        catch (Exception disposeException)
                        {
                            Configuration.Logger.Error("Failed to dispose Glimpse request context handle", disposeException);
                        }
                    }
                }
            }
        }

        private static bool TryGetAspNetRequestResponseAdapter(HttpContextBase httpContext, out IAspNetRequestResponseAdapter aspNetRequestResponseAdapter)
        {
            aspNetRequestResponseAdapter = null;

            GlimpseRequestContextHandle glimpseRequestContextHandle;
            if (TryGetGlimpseRequestContextHandle(httpContext, out glimpseRequestContextHandle))
            {
                GlimpseRequestContext glimpseRequestContext;
                if (GlimpseRuntime.Instance.TryGetRequestContext(glimpseRequestContextHandle.GlimpseRequestId, out glimpseRequestContext))
                {
                    aspNetRequestResponseAdapter = (IAspNetRequestResponseAdapter)glimpseRequestContext.RequestResponseAdapter;
                    return true;
                }

                GlimpseRuntime.Instance.Configuration.Logger.Error("No corresponding GlimpseRequestContext found for GlimpseRequestId '" + glimpseRequestContextHandle.GlimpseRequestId + "'.");
                return false;
            }

            return false;
        }

        private static bool TryGetGlimpseRequestContextHandle(HttpContextBase httpContext, out GlimpseRequestContextHandle glimpseRequestContextHandle)
        {
            glimpseRequestContextHandle = null;

            // question remains whether we should have a Glimpse Request Context handle in case of the execution of an IResource
            if (httpContext.Items.Contains(Constants.GlimpseRequestContextHandle))
            {
                glimpseRequestContextHandle = (GlimpseRequestContextHandle)httpContext.Items[Constants.GlimpseRequestContextHandle];
                if (glimpseRequestContextHandle != null)
                {
                    return true;
                }
            }

            Configuration.Logger.Info("There is no Glimpse request context handle stored inside the httpContext.Items collection.");
            return false;
        }
    }
}