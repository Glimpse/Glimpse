using System.Web.Mvc;
using Glimpse.Net.Plumbing;

namespace Glimpse.Net.Extentions
{
    public static class ControllerExtentions
    {
        internal static void TryWrapActionInvoker(this IController iController)
        {
            //no controller, abort
            if (iController == null) return;

            var controller = iController as Controller;

            //controller is not leveraging an Action invoker, abort
            if (controller == null) return;

            //controller already has GlimpseActionInvoker set, abort
            if (controller.ActionInvoker is GlimpseActionInvoker) return;

            //swap out action invoker
            //TODO: Test wrapping a custom action invoker with a decorator
            if (controller.ActionInvoker is ControllerActionInvoker) controller.ActionInvoker = new GlimpseActionInvoker();
        }
    }
}
