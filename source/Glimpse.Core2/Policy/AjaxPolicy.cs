using System;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    [RuntimePolicy(RuntimeEvent.BeginRequest)]
    public class AjaxPolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            Contract.Requires<ArgumentNullException>(policyContext != null, "policyContext");

            try
            {
                return policyContext.RequestMetadata.RequestIsAjax ? RuntimePolicy.ModifyResponseHeaders : RuntimePolicy.On;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(string.Format(Resources.ExecutePolicyWarning, GetType()), exception);
                return RuntimePolicy.ModifyResponseHeaders;
            }
        }
    }
}