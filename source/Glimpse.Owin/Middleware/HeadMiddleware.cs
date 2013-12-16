using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Microsoft.Owin;

namespace Glimpse.Owin.Middleware
{
    public class HeadMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> innerNext;
        private readonly IDictionary<string, object> serverStore;

        public HeadMiddleware(Func<IDictionary<string, object>, Task> next, IDictionary<string, object> serverStore)
        {
            innerNext = next;
            this.serverStore = serverStore;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            if (!GlimpseRuntime.IsInitialized)
            {
                var config = new GlimpseConfiguration(
                    new OwinResourceEndpointConfiguration(),
                    new ApplicationPersistenceStore(new DictionaryDataStoreAdapter(serverStore as Dictionary<string, object>)));
                GlimpseRuntime.Initialize(config);
            }

            // TODO: this where to check to see if this should handle the request directly (ala glimpse.axd)

            var request = new OwinRequest(environment);
            var response = new OwinResponse(environment);
            var frameworkProvider = new OwinFrameworkProvider(environment, serverStore);

            if (request.Uri.PathAndQuery.StartsWith("/Glimpse.axd", StringComparison.InvariantCultureIgnoreCase))
            {
                await ExecuteResource(frameworkProvider, request.Query);
                return;
            }

            GlimpseRuntime.Instance.BeginRequest(frameworkProvider);
            // Hack's a million!
            var requestId = frameworkProvider.HttpRequestStore.Get<Guid>("__GlimpseRequestId");
            var htmlSnippet = GlimpseRuntime.Instance.GenerateScriptTags(requestId, frameworkProvider);
            response.Body = new PreBodyTagFilter(htmlSnippet, response.Body, Encoding.UTF8, new NullLogger());

            await new OwinResponse(environment).WriteAsync("<!-- Glimpse Start @ " + DateTime.Now.ToLongTimeString() + "-->");
            await innerNext(environment);

            GlimpseRuntime.Instance.EndRequest(frameworkProvider);
            await new OwinResponse(environment).WriteAsync("<!-- Glimpse End#2 @ " + DateTime.Now.ToLongTimeString() + "-->");
        }

        private async Task ExecuteResource(IFrameworkProvider frameworkProvider, IReadableStringCollection queryString)
        {
            if (string.IsNullOrEmpty(queryString["n"]))
            {
                GlimpseRuntime.Instance.ExecuteDefaultResource(frameworkProvider);
            }
            else
            {
                GlimpseRuntime.Instance.ExecuteResource(frameworkProvider, queryString["n"], new ResourceParameters(queryString.ToDictionary(qs => qs.Key, qs => qs.Value.First())));
            }
        }
    }
}