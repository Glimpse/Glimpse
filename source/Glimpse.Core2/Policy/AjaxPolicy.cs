using System;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
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
                policyContext.Logger.Warn(Resources.ExecutePolicyWarning, exception, GetType());
                return RuntimePolicy.ModifyResponseHeaders;
            }
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }
    }
}