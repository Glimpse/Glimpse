using System.Web;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;

namespace $rootnamespace$
{
    public class GlimpseSecurityPolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            var context = policyContext.GetRequestContext<HttpContextBase>();

            //You can perform a check like the one below to control Glimpse's permissions within your application.
            //if (!context.User.IsInRole("Administrator")) 
            //    return RuntimePolicy.Off;

            return RuntimePolicy.On;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }
    }
}