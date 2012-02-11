using System.Web;
using Glimpse.Core2.Extensibility;

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