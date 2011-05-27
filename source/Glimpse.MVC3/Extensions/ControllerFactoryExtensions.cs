using System.Web.Mvc;
using Glimpse.Mvc3.Plumbing;

namespace Glimpse.Mvc3.Extensions
{
    internal static class ControllerFactoryExtentions
    {
        internal static IControllerFactory Wrap(this IControllerFactory iControllerFactory)
        {
            if (iControllerFactory is GlimpseControllerFactory) return iControllerFactory;

            return new GlimpseControllerFactory(iControllerFactory);
        }
    }
}