using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseActionInvoker : ControllerActionInvoker
    {
        protected override ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor, object actionReturnValue)
        {
            var actionResult = base.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);
            return actionResult;
        }

        protected override ControllerDescriptor GetControllerDescriptor(ControllerContext controllerContext)
        {
            var controllerDescriptor = base.GetControllerDescriptor(controllerContext);
            return controllerDescriptor;
        }

        protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
        {
            var actionDescriptor = base.FindAction(controllerContext, controllerDescriptor, actionName);
            return actionDescriptor;
        }


        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var allFilters = controllerContext.HttpContext.Items[GlimpseConstants.AllFilters] as IList<GlimpseFilterCallMetadata>;
            if (allFilters == null) controllerContext.HttpContext.Items[GlimpseConstants.AllFilters] = allFilters = new List<GlimpseFilterCallMetadata>();


            FieldInfo dynField = typeof (ControllerActionInvoker).GetField("_getFiltersThunk", BindingFlags.NonPublic | BindingFlags.Instance);
            var filters = dynField.GetValue(this) as Func<ControllerContext, ActionDescriptor, IEnumerable<Filter>>;
            var filtersEnum = filters(controllerContext, actionDescriptor);

            var filterInfo = base.GetFilters(controllerContext, actionDescriptor);

            var actionFilters = filterInfo.ActionFilters;
            for (int i = 0; i < actionFilters.Count; i++)
            {
                if (!(actionFilters[i] is GlimpseActionFilter))
                {
                    //Wrap it for call logging
                    var innerFilter = filtersEnum.Where(f => f.Instance == actionFilters[i]).SingleOrDefault();
                    var newFilter = new GlimpseActionFilter(actionFilters[i])
                                        {
                                            Filter = innerFilter,
                                            OnActionExecutedGuid = Guid.NewGuid(),
                                            OnActionExecutingGuid = Guid.NewGuid()
                                        };
                    actionFilters[i] = newFilter;

                    //Store metadata for later analysis
                    allFilters.Add(new GlimpseFilterCallMetadata("Action", newFilter.OnActionExecutedGuid, "OnActionExecuted()", innerFilter));
                    allFilters.Add(new GlimpseFilterCallMetadata("Action", newFilter.OnActionExecutingGuid, "OnActionExecuting()", innerFilter));
                }
            }

            var authorizationFilters = filterInfo.AuthorizationFilters;
            for (int i = 0; i < authorizationFilters.Count; i++)
            {
                if (!(authorizationFilters[i] is GlimpseAuthorizationFilter))
                {
                    var innerFilter = filtersEnum.Where(f => f.Instance == authorizationFilters[i]).SingleOrDefault();
                    var newFilter = new GlimpseAuthorizationFilter(authorizationFilters[i])
                                        {
                                            Filter = innerFilter,
                                            Guid = Guid.NewGuid()
                                        };
                    authorizationFilters[i] = newFilter;


                    allFilters.Add(new GlimpseFilterCallMetadata("Authorization", newFilter.Guid, "OnAuthorization()", innerFilter));
                }
            }

            var exceptionFilters = filterInfo.ExceptionFilters;
            for (int i = 0; i < exceptionFilters.Count; i++)
            {
                if (!(exceptionFilters[i] is GlimpseExceptionFilter))
                {
                    var innerFilter = filtersEnum.Where(f => f.Instance == exceptionFilters[i]).SingleOrDefault();
                    var newFilter = new GlimpseExceptionFilter(exceptionFilters[i])
                                        {
                                            Filter = innerFilter,
                                            Guid = Guid.NewGuid()
                                        };

                    exceptionFilters[i] = newFilter;

                    allFilters.Add(new GlimpseFilterCallMetadata("Exception", newFilter.Guid, "OnException()", innerFilter));
                }
            }

            var resultFilters = filterInfo.ResultFilters;
            for (int i = 0; i < resultFilters.Count; i++)
            {
                if (!(resultFilters[i] is GlimpseResultFilter))
                {
                    var innerFilter = filtersEnum.Where(f => f.Instance == resultFilters[i]).SingleOrDefault();
                    var newFilter = new GlimpseResultFilter(resultFilters[i])
                                        {
                                            Filter = innerFilter,
                                            OnResultExecutedGuid = Guid.NewGuid(),
                                            OnResultExecutingGuid = Guid.NewGuid()
                                        };
                    resultFilters[i] = newFilter;

                    allFilters.Add(new GlimpseFilterCallMetadata("Result", newFilter.OnResultExecutedGuid, "OnResultExecuted()", innerFilter));
                    allFilters.Add(new GlimpseFilterCallMetadata("Result", newFilter.OnResultExecutingGuid, "OnResultExecuting()", innerFilter));
                }
            }

            return filterInfo;
        }

        protected override object GetParameterValue(ControllerContext controllerContext, ParameterDescriptor parameterDescriptor)
        {
            var parameterValue = base.GetParameterValue(controllerContext, parameterDescriptor);
            return parameterValue;
        }

        protected override IDictionary<string, object> GetParameterValues(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var parameterValues = base.GetParameterValues(controllerContext, actionDescriptor);
            return parameterValues;
        }

        public override bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            var invokeAction = base.InvokeAction(controllerContext, actionName);
            return invokeAction;
        }

        protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            var invokeActionMethod = base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);
            return invokeActionMethod;
        }

        protected override ActionExecutedContext InvokeActionMethodWithFilters(ControllerContext controllerContext, IList<IActionFilter> filters, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            var invokeActionMethodWithFilters = base.InvokeActionMethodWithFilters(controllerContext, filters,
                                                                                   actionDescriptor, parameters);
            return invokeActionMethodWithFilters;
        }

        protected override void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
        {
            var cc = controllerContext;
            var ar = actionResult;

            base.InvokeActionResult(cc, ar);
        }

        protected override ResultExecutedContext InvokeActionResultWithFilters(ControllerContext controllerContext, IList<IResultFilter> filters, ActionResult actionResult)
        {
            var invokeActionResultWithFilters = base.InvokeActionResultWithFilters(controllerContext, filters, actionResult);
            return invokeActionResultWithFilters;
        }

        protected override AuthorizationContext InvokeAuthorizationFilters(ControllerContext controllerContext, IList<IAuthorizationFilter> filters, ActionDescriptor actionDescriptor)
        {
            var invokeAuthorizationFilters = base.InvokeAuthorizationFilters(controllerContext, filters, actionDescriptor);
            return invokeAuthorizationFilters;
        }

        protected override ExceptionContext InvokeExceptionFilters(ControllerContext controllerContext, IList<IExceptionFilter> filters, Exception exception)
        {
            var invokeExceptionFilters = base.InvokeExceptionFilters(controllerContext, filters, exception);
            return invokeExceptionFilters;
        }
    }
}