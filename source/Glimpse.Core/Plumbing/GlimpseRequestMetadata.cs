using System;

namespace Glimpse.Core.Plumbing
{
    internal class GlimpseRequestMetadata
    {
        public string Method { get; set; }
        public string Json { get; set; }
        public string Browser { get; set; }
        public string ClientName { get; set; }
        public string RequestTime { get; set; }
        public Guid RequestId { get; set; }
        public string IsAjax { get; set; }
        public string Url { get; set; }
    }
}
