using System;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using Glimpse.Core;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class HttpModule : IHttpModule
    {
        private static IConfiguration Configuration { get; set; }

        static HttpModule()
        {
            try
            {
                BuildManager.GetReferencedAssemblies();
                GlimpseRuntime.Initializer.AddInitializationMessage(LoggingLevel.Debug, "Preloaded all referenced assemblies with System.Web.Compilation.BuildManager.GetReferencedAssemblies()");
            }
            catch (Exception exception)
            {
                GlimpseRuntime.Initializer.AddInitializationMessage(LoggingLevel.Error, "Call to System.Web.Compilation.BuildManager.GetReferencedAssemblies() failed.", exception);
            }

            AppDomain.CurrentDomain.DomainUnload += (sender, e) => OnAppDomainUnload((AppDomain)sender);
        }

        private static void OnAppDomainUnload(AppDomain appDomain)
        {
            if (!GlimpseRuntime.IsAvailable)
            {
                return;
            }

            GlimpseRuntime.Instance.Configuration.Logger.Fatal(
               "AppDomain with Id: '{0}' and BaseDirectory: '{1}' has been unloaded. Any in memory data stores have been lost. {2}",
               appDomain.Id,
               appDomain.BaseDirectory,
               "Reason for shutdown => " + HostingEnvironment.ShutdownReason);

            GlimpseRuntime.Instance.Dispose();
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
            if (!GlimpseRuntime.IsAvailable)
            {
                Configuration = Configuration ??
                    ConfigurationFactory.Create(
                        new HttpHandlerEndpointConfiguration(),
                        new InMemoryPersistenceStore(new HttpApplicationStateBaseDataStoreAdapter(httpApplication.Application)),
                        new AspNetCurrentGlimpseRequestIdTracker());

                GlimpseRuntime.Initializer.Initialize(Configuration);
            }

            Func<object, HttpContextWrapper> createHttpContextWrapper = sender => new HttpContextWrapper(((HttpApplication)sender).Context);

            httpApplication.BeginRequest += (context, e) => BeginRequest(createHttpContextWrapper(context));
            httpApplication.PostAcquireRequestState += (context, e) => BeginSessionAccess(createHttpContextWrapper(context));
            httpApplication.PostRequestHandlerExecute += (context, e) => EndSessionAccess(createHttpContextWrapper(context));
            httpApplication.PostReleaseRequestState += (context, e) => EndRequest(createHttpContextWrapper(context));
            httpApplication.PreSendRequestHeaders += (context, e) => SendHeaders(createHttpContextWrapper(context));
        }

        internal void BeginRequest(HttpContextBase httpContext)
        {
            if (!GlimpseRuntime.IsAvailable)
            {
                return;
            }

            var glimpseRequestContextHandle = GlimpseRuntime.Instance.BeginRequest(
                new AspNetRequestResponseAdapter(httpContext, GlimpseRuntime.Instance.Configuration.Logger));

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
            ProcessAspNetRuntimeEvent(
                httpContext,
                glimpseRequestContextHandle =>
                {
                    IGlimpseRequestContext glimpseRequestContext;
                    if (GlimpseRuntime.Instance.TryGetRequestContext(glimpseRequestContextHandle.GlimpseRequestId, out glimpseRequestContext))
                    {
                        ((IAspNetRequestResponseAdapter)glimpseRequestContext.RequestResponseAdapter).PreventSettingHttpResponseHeaders();
                    }
                },
                availabilityOfGlimpseRequestContextHandleIsRequired: false);

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
            if (!GlimpseRuntime.IsAvailable)
            {
                return;
            }

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
                GlimpseRuntime.Instance.Configuration.Logger.Info("There is no Glimpse request context handle stored inside the httpContext.Items collection.");
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