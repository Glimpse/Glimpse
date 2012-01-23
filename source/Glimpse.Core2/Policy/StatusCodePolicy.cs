using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    [RuntimePolicy(RuntimeEvent.EndRequest)]
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
            Contract.Requires<ArgumentNullException>(statusCodeWhitelist != null, "statusCodeWhitelist");

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
                policyContext.Logger.Warn(string.Format(Resources.ExecutePolicyWarning, GetType()), exception);
                return RuntimePolicy.Off;
            }
        }
    }
}