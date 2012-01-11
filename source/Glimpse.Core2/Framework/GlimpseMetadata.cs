using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public class GlimpseMetadata
    {
        public GlimpseMetadata(Guid requestId, IRequestMetadata requestMetadata, IDictionary<string, string> pluginData)
        {
            RequestId = requestId;
            RequestMetadata = requestMetadata;
            PluginData = pluginData;
            DateTime = DateTime.Now;
        }

        public Guid RequestId { get; set; }
        public DateTime DateTime { get; set; }
        public IRequestMetadata RequestMetadata { get; set; }
        public IDictionary<string, string> PluginData { get; set; }
        public string GlimpseClientName //TODO: This should not just be a pass through?
        {
            get { return RequestMetadata.GetCookie(Constants.ControlCookieName); }
        }
    }
}