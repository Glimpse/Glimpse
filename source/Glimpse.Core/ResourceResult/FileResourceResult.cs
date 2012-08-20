using System;
using Glimpse.Core.Extensibility;
// ReSharper disable RedundantUsingDirective
using Glimpse.Core.Extensions;
// ReSharper restore RedundantUsingDirective
using Glimpse.Core.Framework;

namespace Glimpse.Core.ResourceResult
{
    public class FileResourceResult:IResourceResult
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public long CacheDuration { get; set; }
        public CacheSetting? CacheSetting { get; set; }

        public FileResourceResult(byte[] content, string contentType):this(content, contentType, -1, null)
        {
        }

        public FileResourceResult(byte[] content, string contentType, long cacheDuration, CacheSetting? cacheSetting)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (string.IsNullOrEmpty(contentType)) throw new ArgumentNullException("contentType");

            Content = content;
            ContentType = contentType;
            CacheDuration = cacheDuration;
            CacheSetting = cacheSetting;
        }

        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;

            frameworkProvider.SetHttpResponseHeader("Content-Type", ContentType);

#if !DEBUG
            if (CacheSetting.HasValue)
                frameworkProvider.SetHttpResponseHeader("Cache-Control", string.Format("{0}, max-age={1}", CacheSetting.Value.ToDescription(), CacheDuration));
#else
            frameworkProvider.SetHttpResponseHeader("Cache-Control", "no-cache");
#endif

            frameworkProvider.WriteHttpResponse(Content);
        }
    }
}