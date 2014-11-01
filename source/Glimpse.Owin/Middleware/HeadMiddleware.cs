using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Owin.Framework;
using Microsoft.Owin;

namespace Glimpse.Owin.Middleware
{
    public class HeadMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> innerNext;
        private readonly IConfiguration config;

        public HeadMiddleware(Func<IDictionary<string, object>, Task> next, IDictionary<string, object> serverStore)
        {
            innerNext = next;
            config = new Configuration(new UriTemplateResourceEndpointConfiguration(), new InMemoryPersistenceStore(new DictionaryDataStoreAdapter((Dictionary<string, object>)serverStore)));
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
                var requestResponseAdapter = new RequestResponseAdapter(environment);

                using (var glimpseRequestContextHandle = GlimpseRuntime.Instance.BeginRequest(requestResponseAdapter))
                {
                    switch (glimpseRequestContextHandle.RequestHandlingMode)
                    {
                        case RequestHandlingMode.RegularRequest:
                            await ExecuteRegularRequest(glimpseRequestContextHandle, environment, new OwinResponse(environment));
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
            GlimpseRuntime.Instance.ExecuteResource(glimpseRequestContextHandle, queryString[UriTemplateResourceEndpointConfiguration.DefaultResourceNameKey], new ResourceParameters(queryString.ToDictionary(qs => qs.Key, qs => qs.Value.First())));
        }

        private async Task ExecuteRegularRequest(GlimpseRequestContextHandle glimpseRequestContextHandle, IDictionary<string, object> environment, OwinResponse owinResponse)
        {
            try
            {
                await innerNext(environment);
                await owinResponse.Body.FlushAsync();
            }
            finally
            {
                GlimpseRuntime.Instance.EndRequest(glimpseRequestContextHandle);
            }
        }
    }
}