using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Extensions
{
    public static class TabContextExtensions
    {
         public static HttpContextBase GetHttpContext(this ITabContext tabContext)
         {
             return tabContext.GetRequestContext<HttpContextBase>();
         }
    }
}