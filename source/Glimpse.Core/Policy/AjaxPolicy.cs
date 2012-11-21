using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    public class AjaxPolicy : IRuntimePolicy
    {
        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            if (policyContext == null)
            {
                throw new ArgumentNullException("policyContext");
            }

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
    }
}