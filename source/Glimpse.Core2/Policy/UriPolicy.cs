using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    [RuntimePolicy(RuntimeEvent.BeginRequest)]
    public class UriPolicy: ConfigurationSection, IRuntimePolicy
    {
        //TODO: Turn into a proper configuration class
        public IList<Regex> UriBlacklist { get; set; }

        public UriPolicy()
        {
            UriBlacklist = new List<Regex>();
        }

        public UriPolicy(IList<Regex> uriBlacklist)
        {
            Contract.Requires<ArgumentNullException>(uriBlacklist != null, "uriBlacklist");

            UriBlacklist = uriBlacklist;
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                if (UriBlacklist.Count == 0) 
                    return RuntimePolicy.On;

                var uri = policyContext.RequestMetadata.RequestUri;

                if (UriBlacklist.Any(regex => regex.IsMatch(uri)))
                {
                    return RuntimePolicy.Off;
                }

                return RuntimePolicy.On;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(string.Format(Resources.ExecutePolicyWarning, GetType()), exception);
                return RuntimePolicy.Off;
            }
        }
    }
}