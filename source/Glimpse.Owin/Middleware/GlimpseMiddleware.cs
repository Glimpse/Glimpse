using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Microsoft.Owin;

namespace Glimpse.Owin.Middleware
{
    public class GlimpseMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> innerNext;
        private readonly IGlimpseConfiguration config;

        public GlimpseMiddleware(Func<IDictionary<string, object>, Task> next, IDictionary<string, object> serverStore)
        {
            innerNext = next;
            config = new GlimpseConfiguration(
                    new UriTemplateResourceEndpointConfiguration(),
                    new InMemoryPersistenceStore(new DictionaryDataStoreAdapter((Dictionary<string, object>)serverStore))); 
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            if (!GlimpseRuntime.IsInitialized)
            {
                GlimpseRuntime.Initialize(config);
            }

            if (GlimpseRuntime.IsInitialized)
            {
                var request = new OwinRequest(environment);
                var response = new OwinResponse(environment);
                var requestResponseAdapter = new OwinRequestResponseAdapter(environment);

                if (request.Uri.PathAndQuery.StartsWith(config.EndpointBaseUri, StringComparison.InvariantCultureIgnoreCase))
                {
                    await ExecuteResource(requestResponseAdapter, request.Query);
                    return;
                }

                GlimpseRuntime.Instance.BeginRequest(requestResponseAdapter);
                // V2Merge: Hack's a million!
                var requestId = requestResponseAdapter.HttpRequestStore.Get<Guid>("__GlimpseRequestId");
                var htmlSnippet = GlimpseRuntime.Instance.GenerateScriptTags(requestId, requestResponseAdapter);
                response.Body = new PreBodyTagInjectionStream(htmlSnippet, response.Body, Encoding.UTF8, request.Uri.AbsoluteUri, new NullLogger());
            }

            await innerNext(environment);

            if (GlimpseRuntime.IsInitialized)
            {
                GlimpseRuntime.Instance.EndRequest(new OwinRequestResponseAdapter(environment));
            }
        }

        private async Task ExecuteResource(IRequestResponseAdapter requestResponseAdapter, IReadableStringCollection queryString)
        {
            if (string.IsNullOrEmpty(queryString[UriTemplateResourceEndpointConfiguration.DefaultResourceNameKey]))
            {
                GlimpseRuntime.Instance.ExecuteDefaultResource(requestResponseAdapter);
            }
            else
            {
                GlimpseRuntime.Instance.ExecuteResource(
                    requestResponseAdapter, 
                    queryString[UriTemplateResourceEndpointConfiguration.DefaultResourceNameKey], 
                    new ResourceParameters(queryString.ToDictionary(qs => qs.Key, qs => qs.Value.First())));
            }
        }
    }
}