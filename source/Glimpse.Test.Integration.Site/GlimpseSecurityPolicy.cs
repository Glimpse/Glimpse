/*
// Uncomment this class to provide custom runtime policy for Glimpse

using System.Web;
using Glimpse.Core;
using Glimpse.Core.Extensibility;

namespace Glimpse.Test.Integration.Cassini
{
    public class GlimpseSecurityPolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            // You can perform a check like the one below to control Glimpse's permissions within your application.
			// var context = policyContext.GetRequestContext<HttpContextBase>();
            // if (!context.User.IsInRole("Administrator"))
			// {
            //     return RuntimePolicy.Off;
			// }

            return RuntimePolicy.On;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }
    }
}
*/