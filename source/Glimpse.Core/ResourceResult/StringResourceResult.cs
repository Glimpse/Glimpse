using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.ResourceResult
{
    public class StringResourceResult : IResourceResult
    {
        public StringResourceResult() : this(@"text/javascript") { }

        public StringResourceResult(string contentEncodingType)
        {
            if (contentEncodingType == null)
            {
                throw new ArgumentNullException("contentEncodingType");
            }

            ContentEncodingType = contentEncodingType;
        }

        public string ContentEncodingType { get; set; }

        public string Text { get; set; }

        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.RequestResponseAdapter;
            frameworkProvider.SetHttpResponseHeader("Content-Type", ContentEncodingType);
            frameworkProvider.SetHttpResponseHeader("Cache-Control", "no-cache");
            frameworkProvider.WriteHttpResponse(Text);
        }

    }
}
