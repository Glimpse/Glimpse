using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.ResourceResult
{
    public class CacheControlDecorator : ResourceResultDecorator
    {
        private const long NoCaching = -1;

        public CacheControlDecorator(IResourceResult wrappedResourceResult) : this(NoCaching, null, wrappedResourceResult)
        {
        }

        public CacheControlDecorator(long cacheDuration, CacheSetting? cacheSetting, IResourceResult wrappedResourceResult) : base(wrappedResourceResult)
        {
            CacheDuration = cacheDuration;
            CacheSetting = cacheSetting;
        }

        public long CacheDuration { get; set; }

        public CacheSetting? CacheSetting { get; set; }

        protected override void Decorate(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;

#if !DEBUG
            if (CacheSetting.HasValue)
            {
                frameworkProvider.SetHttpResponseHeader("Cache-Control", string.Format("{0}, max-age={1}", CacheSetting.Value.ToDescription(), CacheDuration));
            }
#else
            frameworkProvider.SetHttpResponseHeader("Cache-Control", "no-cache");
#endif
        }
    }
}