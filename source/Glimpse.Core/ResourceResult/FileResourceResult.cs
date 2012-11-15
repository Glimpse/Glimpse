using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.ResourceResult
{
    public class FileResourceResult : IResourceResult
    {
        public FileResourceResult(byte[] content, string contentType)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException("contentType");
            }

            Content = content;
            ContentType = contentType;
        }

        public byte[] Content { get; set; }
        
        public string ContentType { get; set; }

        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;

            frameworkProvider.SetHttpResponseHeader("Content-Type", ContentType);

            frameworkProvider.WriteHttpResponse(Content);
        }
    }
}