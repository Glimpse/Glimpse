using System.Web.Mvc;
using Glimpse.Net.Extensions;

namespace Glimpse.Net.Plumbing
{
    public static class GlimpsePipelineInitiation
    {
        public static void ControllerFactory()
        {
            var controllerBuilder = ControllerBuilder.Current.GetControllerFactory();
            if (controllerBuilder != null) ControllerBuilder.Current.SetControllerFactory(controllerBuilder.Wrap());
        }

        public static void DependencyResolver()
        {
            var dependencyResolver = System.Web.Mvc.DependencyResolver.Current;
            if (dependencyResolver != null && !(dependencyResolver is GlimpseDependencyResolver))
                System.Web.Mvc.DependencyResolver.SetResolver(new GlimpseDependencyResolver(dependencyResolver));
        }
    }
}
