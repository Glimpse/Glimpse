using System;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Extensions;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.ResourceResult
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
            Contract.Requires<ArgumentNullException>(content != null, "content");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(contentType), "contentType");

            Content = content;
            ContentType = contentType;
            CacheDuration = cacheDuration;
            CacheSetting = cacheSetting;
        }

        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;

            frameworkProvider.SetHttpResponseHeader("Content-Type", ContentType);

            if (CacheSetting.HasValue)
                frameworkProvider.SetHttpResponseHeader("Cache-Control", string.Format("{0}, max-age={1}", CacheSetting.Value.ToDescription(), CacheDuration));

            frameworkProvider.WriteHttpResponse(Content);
        }
    }
}