using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Plumbing;

namespace Glimpse.Mvc3.Extensions
{
    internal static class ControllerFactoryExtentions
    {
        internal static IControllerFactory Wrap(this IControllerFactory iControllerFactory, IGlimpseLogger logger)
        {
            if (iControllerFactory is GlimpseControllerFactory) return iControllerFactory;

            return new GlimpseControllerFactory(iControllerFactory, logger);
        }
    }
}