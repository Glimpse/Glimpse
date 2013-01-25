using System;
using System.Collections.Generic;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    public class StatusCodePolicy : IRuntimePolicy, IConfigurable
    {
        public StatusCodePolicy()
        {
            StatusCodeWhitelist = new List<int>();
        }

        public StatusCodePolicy(IList<int> statusCodeWhitelist)
        {
            if (statusCodeWhitelist == null)
            {
                throw new ArgumentNullException("statusCodeWhitelist");
            }

            StatusCodeWhitelist = statusCodeWhitelist;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        public IList<int> StatusCodeWhitelist { get; set; }

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

        public void Configure(Section section)
        {
            foreach (StatusCodeElement item in section.RuntimePolicies.StatusCodes)
            {
                StatusCodeWhitelist.Add(item.StatusCode);
            }
        }
    }
}