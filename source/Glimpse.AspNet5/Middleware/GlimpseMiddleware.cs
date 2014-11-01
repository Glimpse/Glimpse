using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.AspNet5.Framework;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Builder;

namespace Glimpse.AspNet5.Middleware
{
    public class GlimpseMiddleware
    {
        private readonly RequestDelegate innerNext;
        private readonly IConfiguration config;

        public GlimpseMiddleware(RequestDelegate next, IDictionary<string, object> serverStore)
        {
            innerNext = next;
            config = new Configuration(new UriTemplateResourceEndpointConfiguration(), new InMemoryPersistenceStore(new DictionaryDataStoreAdapter((Dictionary<string, object>)serverStore)), new Glimpse.Core.Configuration.Section { EndpointBaseUri = "/Glimpse.axd" } );
        }

        public async Task Invoke(HttpContext context)
        {
            if (!GlimpseRuntime.IsAvailable)
            {
                GlimpseRuntime.Initializer.Initialize(config);
            }

            if (GlimpseRuntime.IsAvailable)
            {
                try
                {
                    var requestResponseAdapter = new RequestResponseAdapter(context);

                    using (var glimpseRequestContextHandle = GlimpseRuntime.Instance.BeginRequest(requestResponseAdapter))
                    {
                        switch (glimpseRequestContextHandle.RequestHandlingMode)
                        {
                            case RequestHandlingMode.RegularRequest:
                                await ExecuteRegularRequest(glimpseRequestContextHandle, context);
                                break;
                            case RequestHandlingMode.ResourceRequest:
                                await ExecuteResourceRequest(glimpseRequestContextHandle, context.Request.Query);
                                break;
                            default:
                                await innerNext(context);
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            else
            {
                await innerNext(context);
            }
        }

        private static async Task ExecuteResourceRequest(GlimpseRequestContextHandle glimpseRequestContextHandle, IReadableStringCollection queryString)
        {
            GlimpseRuntime.Instance.ExecuteResource(glimpseRequestContextHandle, queryString[UriTemplateResourceEndpointConfiguration.DefaultResourceNameKey], new ResourceParameters(queryString.ToDictionary(qs => qs.Key, qs => qs.Value.First())));
        }

        private async Task ExecuteRegularRequest(GlimpseRequestContextHandle glimpseRequestContextHandle, HttpContext context)
        {
            try
            {
                await innerNext(context);
                await context.Response.Body.FlushAsync();
            }
            finally
            {
                GlimpseRuntime.Instance.EndRequest(glimpseRequestContextHandle);
            }
        }
    }
}