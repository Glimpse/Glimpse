using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Extensions
{
    public static class RuntimePolicyContextExtensions
    {
        public static HttpContextBase GetHttpContext(this IRuntimePolicyContext policyContext)
        {
            return policyContext.GetRequestContext<HttpContextBase>();
        }
    }
}
