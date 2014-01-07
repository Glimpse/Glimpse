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
    public class GlimpseMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> innerNext;
        private readonly IDictionary<string, object> serverStore;

        public GlimpseMiddleware(Func<IDictionary<string, object>, Task> next, IDictionary<string, object> serverStore)
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

            var request = new OwinRequest(environment);
            var response = new OwinResponse(environment);
            var frameworkProvider = new OwinFrameworkProvider(environment, serverStore);

            // TODO: Remove hardcode to Glimpse.axd
            if (request.Uri.PathAndQuery.StartsWith("/Glimpse.axd", StringComparison.InvariantCultureIgnoreCase))
            {
                await ExecuteResource(frameworkProvider, request.Query);
                return;
            }

            GlimpseRuntime.Instance.BeginRequest(frameworkProvider);
            // Hack's a million!
            var requestId = frameworkProvider.HttpRequestStore.Get<Guid>("__GlimpseRequestId");
            var htmlSnippet = GlimpseRuntime.Instance.GenerateScriptTags(requestId, frameworkProvider);
            response.Body = new PreBodyTagFilter(htmlSnippet, response.Body, Encoding.UTF8, request.Uri.AbsoluteUri, new NullLogger());

            await innerNext(environment);

            GlimpseRuntime.Instance.EndRequest(frameworkProvider);
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