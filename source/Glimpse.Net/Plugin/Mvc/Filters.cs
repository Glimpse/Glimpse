using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
    public class Filters:IGlimpsePlugin
    {
        public string Name
        {
            get { return "Filters"; }
        }

        public object GetData(HttpApplication application)
        {
            var store = application.Context.Items;
            var mvcFilters = store[GlimpseConstants.Filters] as List<Filter>;
            var mvcActionDescriptor = store[GlimpseConstants.ActionDescriptor] as ActionDescriptor;
            var mvcResult = store[GlimpseConstants.Result] as ActionResult;
            if (mvcFilters == null || mvcActionDescriptor == null || mvcResult == null) return null;

            var result = new List<object[]>
                                    {
                                        new []{ "Ordinal", "Category", "Type", "Method", "Order", "Scope", "Properties" }
                                    };

            var count = 0;
            //List Auth Filters
            IOrderedEnumerable<Filter> authFilters = from f in mvcFilters where f.Instance is IAuthorizationFilter orderby f.Order , f.Scope select f;
            ProcessInfo(authFilters, result, ref count, "Authorization", "OnAuthorization()");
            
            //List OnActionExecuting
            IOrderedEnumerable<Filter> actionFilters = from f in mvcFilters where f.Instance is IActionFilter orderby f.Order, f.Scope select f;
            ProcessInfo(actionFilters, result, ref count, "Action", "OnActionExecuting()");
            
            //List Controller Action
            result.Add(new object[] { count++, "", mvcActionDescriptor.ControllerDescriptor.ControllerType.Name, GetActionName(mvcActionDescriptor), "", "", "", "quiet" });
            
            //List OnActionExecuted
            ProcessInfo(actionFilters.Reverse(), result, ref count, "Action", "OnActionExecuted()");

            //List OnResultExecuting
            IOrderedEnumerable<Filter> responseFilters = from f in mvcFilters where f.Instance is IResultFilter orderby f.Order, f.Scope select f;
            ProcessInfo(responseFilters, result, ref count, "Result", "OnResultExecuting()");

            //List Result
            result.Add(new object[] { count++, "", mvcResult.GetType().Name, "ExecuteResult()", "", "", "", "quiet" });
            
            //List OnResultExecuted
            ProcessInfo(responseFilters.Reverse(), result, ref count, "Result", "OnResultExecuted()");

            //List Exception Filters
            IOrderedEnumerable<Filter> exceptionFilters = from f in mvcFilters where f.Instance is IExceptionFilter orderby f.Order descending , f.Scope descending select f;
            ProcessInfo(exceptionFilters, result, ref count, "Exception", "OnException()");

            if (result.Count == 0) return null;

            return result;
        }

        private string GetActionName(ActionDescriptor mvcActionDescriptor)
        {
            var sb = new StringBuilder(mvcActionDescriptor.ActionName + "(");
            foreach (var parameter in mvcActionDescriptor.GetParameters())
            {
                sb.Append(parameter.ParameterType + " ");
                sb.Append(parameter.ParameterName + ", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString();
        }

        private static void ProcessInfo(IEnumerable<Filter> filters, ICollection<object[]> collection, ref int count, string category, string method)
        {
            foreach (Filter item in filters)
            {

                object obj = (item.Instance as HandleErrorAttribute) 
                    ?? (object) (item.Instance as OutputCacheAttribute);

                collection.Add(new[] { count++, category, item.Instance.GetType().Name, method, item.Order, item.Scope.ToString(), obj });
            }
        }

    }
}
