using Glimpse.Core2.Framework;

namespace Glimpse.AspNet
{
    public class RequestMetadata : IRequestMetadata
    {
        public string RequestUri { get; set; }
        public string RequestHttpMethod { get; set; }
        public string GlimpseClientName { get; set; } //TODO: Should this be here or should cookie access be baked in?
        public int ResponseStatusCode { get; set; }
        public string ResponseContentType { get; set; }
        public string IpAddress { get; set; }
        public bool RequestIsAjax { get; set; }
    }
}