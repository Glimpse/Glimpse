using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core;

namespace Glimpse.Core.Policy
{
    public class ContentTypePolicy:IRuntimePolicy, IConfigurable
    {
        public IList<string> ContentTypeWhitelist { get; set; }

        public ContentTypePolicy()
        {
            ContentTypeWhitelist = new List<string>();
        }

        public ContentTypePolicy(IList<string> contentTypeWhitelist)
        {
            if (contentTypeWhitelist == null) throw new ArgumentNullException("contentTypeWhitelist");

            ContentTypeWhitelist = contentTypeWhitelist;
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                var contentType = policyContext.RequestMetadata.ResponseContentType.ToLowerInvariant();
                //support for the following content type strings: "text/html" & "text/html; charset=utf-8"
                return ContentTypeWhitelist.Any(ct => contentType.Contains(ct.ToLowerInvariant())) ? RuntimePolicy.On : RuntimePolicy.Off;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(Resources.ExecutePolicyWarning, exception, GetType());
                return RuntimePolicy.Off;
            }
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        public void Configure(GlimpseSection section)
        {
            ContentTypeWhitelist = new List<string>();

            foreach (ContentTypeElement item in section.RuntimePolicies.ContentTypes)
            {
                ContentTypeWhitelist.Add(item.ContentType);
            }
        }
    }
}