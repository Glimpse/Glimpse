using System;
using System.Web;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Policy
{
    public class LocalPolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            if (policyContext == null) throw new ArgumentNullException("policyContext");

            var httpContext = policyContext.GetRequestContext<HttpContextBase>();

            if (httpContext.Request.IsLocal)
                return RuntimePolicy.On;

            return RuntimePolicy.Off;
        }

        public RuntimeEvent ExecuteOn { 
            get { return RuntimeEvent.BeginRequest; }
        }
    }
}