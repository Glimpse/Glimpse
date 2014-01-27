using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
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

                using (var glimpseRequestContextHandle = GlimpseRuntime.Instance.BeginRequest(requestResponseAdapter))
                {
                    if (glimpseRequestContextHandle.RequestHandlingMode == RequestHandlingMode.Unhandled)
                    {
                        await innerNext(environment);
                        return;
                    }

                    try
                    {
                        if (glimpseRequestContextHandle.RequestHandlingMode == RequestHandlingMode.ResourceRequest)
                        {
                            await ExecuteResource(glimpseRequestContextHandle, request.Query);
                        }
                        else
                        {
                            // V2Merge: Hack's a million!
#warning Even with this hack, it seems wrong, as the scripts will always be injected independent of the RuntimePolicy (only DisplayGlimpseClient should render it, and we only know that at the end)
                            var htmlSnippet = GlimpseRuntime.Instance.GenerateScriptTags(glimpseRequestContextHandle.GlimpseRequestId, requestResponseAdapter);
                            response.Body = new PreBodyTagInjectionStream(htmlSnippet, response.Body, Encoding.UTF8, request.Uri.AbsoluteUri, new NullLogger());

                            await innerNext(environment);
                        }
                    }
                    finally
                    {
                        GlimpseRuntime.Instance.EndRequest(glimpseRequestContextHandle);
                    }
                }
            }
            else
            {
                await innerNext(environment);
            }
        }

        private static async Task ExecuteResource(GlimpseRequestContextHandle glimpseRequestContextHandle, IReadableStringCollection queryString)
        {
            if (string.IsNullOrEmpty(queryString[UriTemplateResourceEndpointConfiguration.DefaultResourceNameKey]))
            {
                GlimpseRuntime.Instance.ExecuteDefaultResource(glimpseRequestContextHandle);
            }
            else
            {
                GlimpseRuntime.Instance.ExecuteResource(
                    glimpseRequestContextHandle,
                    queryString[UriTemplateResourceEndpointConfiguration.DefaultResourceNameKey],
                    new ResourceParameters(queryString.ToDictionary(qs => qs.Key, qs => qs.Value.First())));
            }
        }
    }
}