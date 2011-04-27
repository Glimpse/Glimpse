using System;
using System.Collections.Generic;
using System.Linq;
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
            var st = serviceType;

            var service = DependencyResolver.GetService(st);

            //Wrap up controller factory
            var iControllerFactory = service as IControllerFactory;
            if (iControllerFactory != null) return iControllerFactory.Wrap();

            //Add action invoker to controller
            var iController = service as IController;
            if (iController != null) return iController.TrySetActionInvoker();

            return service;

        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var results = DependencyResolver.GetServices(serviceType);

            if (results.Count() > 0)
            {
                //TODO:Test the objects if needed here...
            }

            return results;
        }
    }
}