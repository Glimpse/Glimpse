using System;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensions;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public class JsonResourceResult:IResourceResult
    {
        public object Data { get; set; }
        public string ContentType { get; set; }
        public long CacheDuration { get; set; }
        public CacheSetting? CacheSetting { get; set; }

        public JsonResourceResult(object data, string contentType):this(data, contentType, -1, null)
        {
        }

        public JsonResourceResult(object data, string contentType, long cacheDuration, CacheSetting? cacheSetting)
        {
            Contract.Requires<ArgumentNullException>(data != null, "data");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(contentType), "contentType");

            Data = data;
            ContentType = contentType;
            CacheDuration = cacheDuration;
            CacheSetting = cacheSetting;
        }

        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;
            var serializer = context.Serializer;

            var result = serializer.Serialize(Data);

            frameworkProvider.SetHttpResponseHeader("Content-Type", ContentType);

            //TODO: Refactor to leverage a CachableResourceResult?
            if (CacheSetting.HasValue)
                frameworkProvider.SetHttpResponseHeader("Cache-Control", string.Format("{0}, max-age={1}", CacheSetting.Value.ToDescription(), CacheDuration));

            frameworkProvider.WriteHttpResponse(result);
        }
    }
}