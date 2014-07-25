using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Builder;

namespace Glimpse.ProjectK.Middleware
{
    public class GlimpseMiddleware
    {
        private readonly RequestDelegate innerNext;
        private readonly IConfiguration config;

        public GlimpseMiddleware(RequestDelegate next, IDictionary<string, object> serverStore)
        {
            innerNext = next;
            config = new Configuration(
                    new UriTemplateResourceEndpointConfiguration(),
                    new InMemoryPersistenceStore(new DictionaryDataStoreAdapter((Dictionary<string, object>)serverStore)));
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
            GlimpseRuntime.Instance.ExecuteResource(
                glimpseRequestContextHandle, 
                queryString[UriTemplateResourceEndpointConfiguration.DefaultResourceNameKey], 
                new ResourceParameters(queryString.ToDictionary(qs => qs.Key, qs => qs.Value.First())));
        }

        private async Task ExecuteRegularRequest(GlimpseRequestContextHandle glimpseRequestContextHandle, HttpContext context)
        {
            try
            {
                // V2Merge: Hack's a million!
#warning Even with this hack, it seems wrong, as the scripts will always be injected independent of the RuntimePolicy (only DisplayGlimpseClient should render it, and we only know that at the end)
                var htmlSnippet = GlimpseRuntime.Instance.GenerateScriptTags(glimpseRequestContextHandle);
                context.Response.Body = new PreBodyTagInjectionStream(htmlSnippet, context.Response.Body, Encoding.UTF8, context.Request.Host + (context.Request.Path + context.Request.QueryString), GlimpseRuntime.Instance.Configuration.Logger);

                await innerNext(context);
            }
            finally
            {
                GlimpseRuntime.Instance.EndRequest(glimpseRequestContextHandle);
            }
        }
    }
}