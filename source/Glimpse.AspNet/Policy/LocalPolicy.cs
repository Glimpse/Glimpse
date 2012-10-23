using System;
using System.Web;
using Glimpse.Core;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Policy
{
    public class LocalPolicy : IRuntimePolicy
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

            var httpContext = policyContext.GetRequestContext<HttpContextBase>();

            if (httpContext.Request.IsLocal)
            {
                return RuntimePolicy.On;
            }

            return RuntimePolicy.Off;
        }
    }
}