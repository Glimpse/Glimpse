using Glimpse.Core.Extensibility;
// ReSharper disable RedundantUsingDirective
//   Glimpse.Core.Extensions.EnumExtensions.ToDescription used in !DEBUG section.
using Glimpse.Core.Extensions;
// ReSharper restore RedundantUsingDirective
using Glimpse.Core.Framework;

namespace Glimpse.Core.ResourceResult
{
    /// <summary>
    /// The <see cref="ResourceResultDecorator"/> implementation responsible providing Http caching to <see cref="IResourceResult"/> implementations via the <c>Cache-Control</c> Http response header.
    /// </summary>
    public class CacheControlDecorator : ResourceResultDecorator
    {
        private const long NoCaching = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheControlDecorator" /> class.
        /// </summary>
        /// <param name="wrappedResourceResult">The wrapped resource result.</param>
        public CacheControlDecorator(IResourceResult wrappedResourceResult) : this(NoCaching, null, wrappedResourceResult)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheControlDecorator" /> class.
        /// </summary>
        /// <param name="cacheDuration">Duration of the cache.</param>
        /// <param name="cacheSetting">The cache setting.</param>
        /// <param name="wrappedResourceResult">The wrapped resource result.</param>
        public CacheControlDecorator(long cacheDuration, CacheSetting? cacheSetting, IResourceResult wrappedResourceResult) : base(wrappedResourceResult)
        {
            CacheDuration = cacheDuration;
            CacheSetting = cacheSetting;
        }

        /// <summary>
        /// Gets or sets the duration of the cache.
        /// </summary>
        /// <value>
        /// The duration of the cache in seconds.
        /// </value>
        public long CacheDuration { get; set; }

        /// <summary>
        /// Gets or sets the cache directive.
        /// </summary>
        /// <value>
        /// The cache directive.
        /// </value>
        public CacheSetting? CacheSetting { get; set; }

        /// <summary>
        /// Decorates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
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