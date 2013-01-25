using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    public class UriPolicy : IRuntimePolicy, IConfigurable
    {
        public UriPolicy()
        {
            UriBlacklist = new List<Regex>();
        }

        public UriPolicy(IList<Regex> uriBlacklist)
        {
            if (uriBlacklist == null)
            {
                throw new ArgumentNullException("uriBlacklist");
            }

            UriBlacklist = uriBlacklist;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }

        public IList<Regex> UriBlacklist { get; set; }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                if (UriBlacklist.Count == 0)
                {
                    return RuntimePolicy.On;
                }

                var uri = policyContext.RequestMetadata.RequestUri;

                if (UriBlacklist.Any(regex => regex.IsMatch(uri)))
                {
                    return RuntimePolicy.Off;
                }

                return RuntimePolicy.On;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(Resources.ExecutePolicyWarning, exception, GetType());
                return RuntimePolicy.Off;
            }
        }

        public void Configure(Section section)
        {
            foreach (RegexElement item in section.RuntimePolicies.Uris)
            {
                UriBlacklist.Add(item.Regex);
            }
        }
    }
}