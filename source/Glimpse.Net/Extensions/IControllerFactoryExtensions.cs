using System.Web.Mvc;
using Glimpse.Net.Plumbing;

namespace Glimpse.Net.Extensions
{
    public static class IControllerFactoryExtentions
    {
        public static IControllerFactory Wrap(this IControllerFactory iControllerFactory)
        {
            if (iControllerFactory is GlimpseControllerFactory) return iControllerFactory;

            return new GlimpseControllerFactory(iControllerFactory);
        }
    }
}