using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
    public class GlobalFilters:IGlimpsePlugin
    {
        public string Name
        {
            get { return "Global Filters"; }
        }

        public object GetData(HttpApplication application)
        {
            var result = new List<object[]>
                                    {
                                        new []{ "Ordinal", "Type", "Category", "Order", "Scope" }
                                    };

            var count = 0;
            var mvcFilters = System.Web.Mvc.GlobalFilters.Filters;

            IOrderedEnumerable<Filter> authFilters = from f in mvcFilters where f.Instance is IAuthorizationFilter orderby f.Order , f.Scope select f;
            ProcessInfo(authFilters, result, ref count,"Authorization");

            IOrderedEnumerable<Filter> actionFilters = from f in mvcFilters where f.Instance is IActionFilter orderby f.Order, f.Scope select f;
            ProcessInfo(actionFilters, result, ref count, "Action");

            IOrderedEnumerable<Filter> responseFilters = from f in mvcFilters where f.Instance is IResultFilter orderby f.Order, f.Scope select f;
            ProcessInfo(responseFilters, result, ref count, "Result");

            IOrderedEnumerable<Filter> exceptionFilters = from f in mvcFilters where f.Instance is IExceptionFilter orderby f.Order, f.Scope select f;
            ProcessInfo(exceptionFilters, result, ref count, "Exception");

            var filters = application.Context.Items[GlimpseConstants.Filters] as List<Filter>;
            ProcessInfo(filters, result, ref count, "Other");


            if (result.Count == 0) return null;


            return result;
        }

        private static void ProcessInfo(IEnumerable<Filter> filters, ICollection<object[]> collection, ref int count, string category)
        {
            foreach (Filter item in filters)
            {
                collection.Add(new object[]{count++, item.Instance.GetType().ToString(), category, item.Order, item.Scope.ToString()});
            }
        }

    }
}
