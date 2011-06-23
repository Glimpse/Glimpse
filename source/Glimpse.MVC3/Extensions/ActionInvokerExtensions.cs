using System;
using System.Web.Mvc;
using Castle.DynamicProxy;

namespace Glimpse.Mvc3.Extensions
{
    internal static class ActionInvokerExtensions
    {
        internal static bool CanSupportDynamicProxy(this IActionInvoker actionInvoker)
        {
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
                    //TODO: Add logging

                return result;
            }

            //TODO add logging warnings.Add(new NotAControllerActionInvokerWarning(actionInvoker));
            return false;
        }
    }
}
