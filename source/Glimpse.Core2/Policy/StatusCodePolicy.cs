using System;
using System.Collections.Generic;
using System.Configuration;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    public class StatusCodePolicy:ConfigurationSection, IRuntimePolicy
    {
        //TODO: Turn into a proper configuration class
        public IList<int> StatusCodeWhitelist { get; set; }

        public StatusCodePolicy()
        {
            StatusCodeWhitelist = new List<int>{200, 301, 302};
        }

        public StatusCodePolicy(IList<int> statusCodeWhitelist)
        {
            if (statusCodeWhitelist == null) throw new ArgumentNullException("statusCodeWhitelist");

            StatusCodeWhitelist = statusCodeWhitelist;
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                var statusCode = policyContext.RequestMetadata.ResponseStatusCode;
                return StatusCodeWhitelist.Contains(statusCode) ? RuntimePolicy.On : RuntimePolicy.Off;
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
    }
}