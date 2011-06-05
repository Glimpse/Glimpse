using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Plumbing
{
    public class InProcStackMetadataStore:IGlimpseMetadataStore
    {
        private GlimpseConfiguration Configuration { get; set; }

        public InProcStackMetadataStore(GlimpseConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Persist(string json, HttpContextBase context)
        {
            if (Configuration.RequestLimit <= 0) return;

            var store = context.Application;

            //TODO: Turn Queue into provider model so it can be stored in SQL/Caching layer for farms
            var queue = store[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;

            if (queue == null)
                store[GlimpseConstants.JsonQueue] =
                    queue = new Queue<GlimpseRequestMetadata>(Configuration.RequestLimit);

            if (queue.Count == Configuration.RequestLimit) queue.Dequeue();

            var browser = context.Request.Browser;
            queue.Enqueue(new GlimpseRequestMetadata
            {
                Browser = string.Format("{0} {1}", browser.Browser, browser.Version),
                ClientName = context.GetClientName(),
                Json = json,
                RequestTime = DateTime.Now.ToLongTimeString(),
                RequestId = context.GetRequestId(),
                IsAjax = context.IsAjax().ToString(),
                Url = context.Request.RawUrl,
                Method = context.Request.HttpMethod
            });
        }
    }
}
