using System;
using System.Web;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Core.Extensions;
using Glimpse.Mvc3.Warning;

namespace Glimpse.Mvc3.Extensions
{
    internal static class ActionInvokerExtensions
    {
        internal static bool CanSupportDynamicProxy(this IActionInvoker actionInvoker)
        {
            var warnings = HttpContext.Current.GetWarnings();

            if (actionInvoker is ControllerActionInvoker)//TODO: What changes for AsyncControllerActionInvoker?
            {
                //Make sure there is a parameterless constructor and the type is not sealed
                var actionInvokerType = actionInvoker.GetType();
                var proxy = actionInvoker as IProxyTargetAccessor;
                var defaultConstructor = actionInvokerType.GetConstructor(new Type[] { });

                var result = (!actionInvokerType.IsSealed &&
                        defaultConstructor != null &&
                        proxy == null);

                if (!result)
                    warnings.Add(new NotProxyableWarning(actionInvoker));

                return result;
            }

            warnings.Add(new NotAControllerActionInvokerWarning(actionInvoker));
            return false;
        }
    }
}
