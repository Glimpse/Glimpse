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
        private readonly IConfiguration config;

        public GlimpseMiddleware(Func<IDictionary<string, object>, Task> next, IDictionary<string, object> serverStore)
        {
            innerNext = next;
            config = new Configuration(
                    new UriTemplateResourceEndpointConfiguration(),
                    new InMemoryPersistenceStore(new DictionaryDataStoreAdapter((Dictionary<string, object>)serverStore)));
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            if (!GlimpseRuntime.IsAvailable)
            {
                GlimpseRuntime.Initializer.Initialize(config);
            }

            if (GlimpseRuntime.IsAvailable)
            {
                var request = new OwinRequest(environment);
                var response = new OwinResponse(environment);
                var requestResponseAdapter = new OwinRequestResponseAdapter(environment);

                using (var glimpseRequestContextHandle = GlimpseRuntime.Instance.BeginRequest(requestResponseAdapter))
                {
                    switch (glimpseRequestContextHandle.RequestHandlingMode)
                    {
                        case RequestHandlingMode.RegularRequest:
                            await ExecuteRegularRequest(glimpseRequestContextHandle, request, response, environment);
                            break;
                        case RequestHandlingMode.ResourceRequest:
                            await ExecuteResourceRequest(glimpseRequestContextHandle, request.Query);
                            break;
                        default:
                            await innerNext(environment);
                            break;
                    }
                }
            }
            else
            {
                await innerNext(environment);
            }
        }

        private static async Task ExecuteResourceRequest(GlimpseRequestContextHandle glimpseRequestContextHandle, IReadableStringCollection queryString)
        {
            GlimpseRuntime.Instance.ExecuteResource(
                glimpseRequestContextHandle, 
                queryString[UriTemplateResourceEndpointConfiguration.DefaultResourceNameKey], 
                new ResourceParameters(queryString.ToDictionary(qs => qs.Key, qs => qs.Value.First())));
        }

        private async Task ExecuteRegularRequest(GlimpseRequestContextHandle glimpseRequestContextHandle, IOwinRequest request, IOwinResponse response, IDictionary<string, object> environment)
        {
            try
            {
                 response.Body = new PreBodyTagInjectionStream(
                        () => GlimpseRuntime.Instance.GenerateScriptTags(glimpseRequestContextHandle),
                        response.Body,
                        () => Encoding.UTF8,
                        () => request.Uri.AbsoluteUri,
                        GlimpseRuntime.Instance.Configuration.Logger);

                await innerNext(environment);
            }
            finally
            {
                GlimpseRuntime.Instance.EndRequest(glimpseRequestContextHandle);
            }
        }
    }
}