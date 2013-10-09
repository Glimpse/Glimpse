using System;
using System.Web;
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class GlimpseRuntimeWrapper
    {
        private const string EventHandledCacheKeyFormat = "__GlimpseRuntimeWrapperHandledHttpApplication{0}Event";
        private readonly IFrameworkProvider frameworkProvider;
        private readonly IGlimpseRuntime glimpseRuntime;
        private readonly ILogger logger;
        private readonly bool writeRequestFlowHandlingLogs;

        public GlimpseRuntimeWrapper(IFrameworkProvider frameworkProvider, IGlimpseRuntime glimpseRuntime, ILogger logger, bool writeRequestFlowHandlingLogs)
        {
            if (frameworkProvider == null)
            {
                throw new ArgumentNullException("frameworkProvider");
            }

            this.frameworkProvider = frameworkProvider;

            if (glimpseRuntime == null)
            {
                throw new ArgumentNullException("glimpseRuntime");
            }

            this.glimpseRuntime = glimpseRuntime;

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.logger = logger;
            this.writeRequestFlowHandlingLogs = writeRequestFlowHandlingLogs;
        }

        public void Initialize(HttpApplicationBase httpApplication)
        {
            if (httpApplication == null)
            {
                throw new ArgumentNullException("httpApplication");
            }

            if (this.glimpseRuntime.IsInitialized || this.glimpseRuntime.Initialize())
            {
                // we explicitly unsubscribe first in case this method is called multiple times for the same httpApplication,
                // which is rarely going to happen, but never the less it happened before (GitHub issue #341)
                httpApplication.BeginRequest -= this.OnBeginRequest;
                httpApplication.BeginRequest += this.OnBeginRequest;
                httpApplication.PostAcquireRequestState -= this.OnPostAcquireRequestState;
                httpApplication.PostAcquireRequestState += this.OnPostAcquireRequestState;
                httpApplication.PostRequestHandlerExecute -= this.OnPostRequestHandlerExecute;
                httpApplication.PostRequestHandlerExecute += this.OnPostRequestHandlerExecute;
                httpApplication.PostReleaseRequestState -= this.OnPostReleaseRequestState;
                httpApplication.PostReleaseRequestState += this.OnPostReleaseRequestState;
                httpApplication.PreSendRequestHeaders -= this.OnPreSendRequestHeaders;
                httpApplication.PreSendRequestHeaders += this.OnPreSendRequestHeaders;
            }
        }

        /// <summary>
        /// Asks the wrapped <see cref="IGlimpseRuntime"/> to execute the default resource.
        /// </summary>
        public void ExecuteDefaultResource()
        {
            if (this.writeRequestFlowHandlingLogs)
            {
                this.logger.Debug(Resources.GlimpseRuntimeWrapperExecuteDefaultResource);
            }

            this.glimpseRuntime.ExecuteDefaultResource();
        }

        /// <summary>
        /// Asks the wrapped <see cref="IGlimpseRuntime"/> to execute the resource.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if either parameter is <c>null</c>.</exception>
        public void ExecuteResource(string resourceName, ResourceParameters parameters)
        {
            if (this.writeRequestFlowHandlingLogs)
            {
                this.logger.Debug(Resources.GlimpseRuntimeWrapperExecuteResource, resourceName);
            }

            this.glimpseRuntime.ExecuteResource(resourceName, parameters);
        }

        private void HandleEventConditionally(object sender, string eventName, Action eventHandler)
        {
            string eventHandlingKey = string.Format(EventHandledCacheKeyFormat, eventName);

            // Under normal circumstances each HttpApplication event is only raised once during the processing of a given HttpRequest, unfortunately
            // there are scenarios when this is not always guaranteed (see Glimpse issue #341 on GitHub (https://github.com/Glimpse/Glimpse/issues/341)), 
            // therefore we'll protect the Glimpse Runtime from having to deal with those rare cases.

            if (this.frameworkProvider.HttpRequestStore.Contains(eventHandlingKey))
            {
                if (this.writeRequestFlowHandlingLogs)
                {
                    this.logger.Warn(Resources.GlimpseRuntimeWrapperAlreadyHandledEvent, eventName, sender.GetType(), sender.GetHashCode());
                }

                return;
            }

            this.frameworkProvider.HttpRequestStore.Set(eventHandlingKey, true);

            if (this.writeRequestFlowHandlingLogs)
            {
                this.logger.Debug(Resources.GlimpseRuntimeWrapperHandlingEvent, eventName, sender.GetType(), sender.GetHashCode());
            }

            eventHandler();
        }

        private void OnBeginRequest(object context, EventArgs e)
        {
            this.HandleEventConditionally(
                context,
                "BeginRequest",
                this.glimpseRuntime.BeginRequest);
        }

        private void OnPostAcquireRequestState(object context, EventArgs e)
        {
            this.HandleEventConditionally(
                context,
                "PostAcquireRequestState",
                this.glimpseRuntime.BeginSessionAccess);
        }

        private void OnPostRequestHandlerExecute(object context, EventArgs e)
        {
            this.HandleEventConditionally(
                context,
                "PostRequestHandlerExecute",
                this.glimpseRuntime.EndSessionAccess);
        }

        private void OnPostReleaseRequestState(object context, EventArgs e)
        {
            this.HandleEventConditionally(
                context,
                "PostReleaseRequestState",
                this.glimpseRuntime.EndRequest);
        }

        private void OnPreSendRequestHeaders(object context, EventArgs e)
        {
            this.HandleEventConditionally(
                context,
                "PreSendRequestHeaders",
                () => new HttpContextWrapper(((HttpApplication)context).Context).HeadersSent(true));
        }
    }
}