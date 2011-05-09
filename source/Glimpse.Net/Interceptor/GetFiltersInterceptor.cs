using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Net.Extensions;
using Glimpse.Net.Plumbing;

namespace Glimpse.Net.Interceptor
{
    internal class GetFiltersInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //Intercepting: GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)

            var controllerContext = (ControllerContext) invocation.Arguments[0];
            var actionDescriptor = (ActionDescriptor)invocation.Arguments[1];

            var allFilters = controllerContext.FiltersStore();

            FieldInfo dynField = typeof(ControllerActionInvoker).GetField("_getFiltersThunk", BindingFlags.NonPublic | BindingFlags.Instance);
            var filters = dynField.GetValue(invocation.InvocationTarget) as Func<ControllerContext, ActionDescriptor, IEnumerable<Filter>>;
            var filtersEnum = filters(controllerContext, actionDescriptor);

            invocation.Proceed();

            var filterInfo = (FilterInfo)invocation.ReturnValue;

            var isChild = controllerContext.IsChildAction;

            var actionFilters = filterInfo.ActionFilters;
            for (int i = 0; i < actionFilters.Count; i++)
            {
                var originalFilter = actionFilters[i];
                if (!(originalFilter is GlimpseActionFilter))
                {
                    //Wrap it for call logging

                    var innerFilter = filtersEnum.Where(f => f.Instance == originalFilter).SingleOrDefault();
                    var newFilter = new GlimpseActionFilter(originalFilter)
                    {
                        Filter = innerFilter,
                        OnActionExecutedGuid = Guid.NewGuid(),
                        OnActionExecutingGuid = Guid.NewGuid()
                    };

                    //Store metadata for later analysis
                    allFilters.Add(new GlimpseFilterCallMetadata("Action", newFilter.OnActionExecutedGuid, "OnActionExecuted()", innerFilter, isChild, originalFilter));
                    allFilters.Add(new GlimpseFilterCallMetadata("Action", newFilter.OnActionExecutingGuid, "OnActionExecuting()", innerFilter, isChild, originalFilter));

                    actionFilters[i] = newFilter;
                }
            }

            var authorizationFilters = filterInfo.AuthorizationFilters;
            for (int i = 0; i < authorizationFilters.Count; i++)
            {
                var originalFilter = authorizationFilters[i];
                if (!(originalFilter is GlimpseAuthorizationFilter))
                {
                    var innerFilter = filtersEnum.Where(f => f.Instance == originalFilter).SingleOrDefault();
                    var newFilter = new GlimpseAuthorizationFilter(originalFilter)
                    {
                        Filter = innerFilter,
                        Guid = Guid.NewGuid()
                    };

                    allFilters.Add(new GlimpseFilterCallMetadata("Authorization", newFilter.Guid, "OnAuthorization()", innerFilter, isChild, originalFilter));

                    authorizationFilters[i] = newFilter;
                }
            }

            var exceptionFilters = filterInfo.ExceptionFilters;
            for (int i = 0; i < exceptionFilters.Count; i++)
            {
                var originalFilter = exceptionFilters[i];
                if (!(originalFilter is GlimpseExceptionFilter))
                {
                    var innerFilter = filtersEnum.Where(f => f.Instance == originalFilter).SingleOrDefault();
                    var newFilter = new GlimpseExceptionFilter(originalFilter)
                    {
                        Filter = innerFilter,
                        Guid = Guid.NewGuid()
                    };

                    allFilters.Add(new GlimpseFilterCallMetadata("Exception", newFilter.Guid, "OnException()", innerFilter, isChild, originalFilter));

                    exceptionFilters[i] = newFilter;
                }
            }

            var resultFilters = filterInfo.ResultFilters;
            for (int i = 0; i < resultFilters.Count; i++)
            {
                var originalFilter = resultFilters[i];
                if (!(originalFilter is GlimpseResultFilter))
                {
                    var innerFilter = filtersEnum.Where(f => f.Instance == originalFilter).SingleOrDefault();
                    var newFilter = new GlimpseResultFilter(originalFilter)
                    {
                        Filter = innerFilter,
                        OnResultExecutedGuid = Guid.NewGuid(),
                        OnResultExecutingGuid = Guid.NewGuid()
                    };

                    allFilters.Add(new GlimpseFilterCallMetadata("Result", newFilter.OnResultExecutedGuid, "OnResultExecuted()", innerFilter, isChild, originalFilter));
                    allFilters.Add(new GlimpseFilterCallMetadata("Result", newFilter.OnResultExecutingGuid, "OnResultExecuting()", innerFilter, isChild, originalFilter));

                    resultFilters[i] = newFilter;
                }
            }

            invocation.ReturnValue = filterInfo;
        }
    }
}