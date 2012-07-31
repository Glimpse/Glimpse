using System;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
// ReSharper disable RedundantUsingDirective
using Glimpse.Core2.Extensions;
// ReSharper restore RedundantUsingDirective

namespace Glimpse.Core2.ResourceResult
{
    public class JsonResourceResult:IResourceResult
    {
        public object Data { get; set; }
        public string Callback { get; set; }
        public string ContentType { get; set; }
        public long CacheDuration { get; set; }
        public CacheSetting? CacheSetting { get; set; }
        private const long NoCaching = -1;

        public JsonResourceResult(object data):this(data, null, NoCaching, null){}

        public JsonResourceResult(object data, string callback): this(data, callback, NoCaching, null){}

        public JsonResourceResult(object data, long cacheDuration, CacheSetting? cacheSetting):this(data, null, cacheDuration, cacheSetting){}

        public JsonResourceResult(object data, string callback, long cacheDuration, CacheSetting? cacheSetting)
        {
            
            if (data == null) throw new ArgumentNullException("data");

            Data = data;
            Callback = callback;
            ContentType = string.IsNullOrEmpty(callback) ? @"application/json" : @"application/x-javascript";
            CacheDuration = cacheDuration;
            CacheSetting = cacheSetting;
        }

        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;
            var serializer = context.Serializer;

            var result = serializer.Serialize(Data);

            frameworkProvider.SetHttpResponseHeader("Content-Type", ContentType);

#if !DEBUG
            if (CacheSetting.HasValue)
                frameworkProvider.SetHttpResponseHeader("Cache-Control", string.Format("{0}, max-age={1}", CacheSetting.Value.ToDescription(), CacheDuration));
#else
            frameworkProvider.SetHttpResponseHeader("Cache-Control", "no-cache");
#endif

            var toWrite = string.IsNullOrEmpty(Callback) ? result : string.Format("{0}({1});", Callback, result);

            frameworkProvider.WriteHttpResponse(toWrite);
        }
    }
}