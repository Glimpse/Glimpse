using System;
using System.Collections.Generic;
using System.Configuration;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Resource;

namespace Glimpse.Core2.Policy
{
    public class ContentTypePolicy:ConfigurationSection, IRuntimePolicy
    {
        //TODO: Turn into a proper configuration class
        public IList<string> ContentTypeWhitelist { get; set; }

        public ContentTypePolicy()
        {
            ContentTypeWhitelist = new List<string>
                                       {
                                           @"text/html",
                                           @"application/json"
                                       };
        }

        public ContentTypePolicy(IList<string> contentTypeWhitelist)
        {
            ContentTypeWhitelist = contentTypeWhitelist;
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                var contentType = policyContext.RequestMetadata.ResponseContentType;
                return ContentTypeWhitelist.Contains(contentType) ? RuntimePolicy.On : RuntimePolicy.ModifyResponseHeaders;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(string.Format(Resources.ExecutePolicyWarning, GetType()), exception);
                return RuntimePolicy.ModifyResponseHeaders;
            }
        }
    }
}