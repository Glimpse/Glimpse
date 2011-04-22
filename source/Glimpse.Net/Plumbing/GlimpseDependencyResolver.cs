using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Net.Extensions;

namespace Glimpse.Net.Plumbing
{
    //TODO:TEST ME
    public class GlimpseDependencyResolver : IDependencyResolver
    {
        public GlimpseDependencyResolver(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
        }

        public IDependencyResolver DependencyResolver { get; set; }

        public object GetService(Type serviceType)
        {
            var result = DependencyResolver.GetService(serviceType);

            var controller = result as Controller;
            if (controller != null)
            {
                controller.TryWrapActionInvoker();
                return result;
            }

            var controllerFactory = result as IControllerFactory;
            if (controllerFactory != null)
            {
                if (!(controllerFactory is GlimpseControllerFactory))
                    return new GlimpseControllerFactory(controllerFactory);
            }

            return result;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var result = DependencyResolver.GetServices(serviceType);
            return result;
        }
    }
}