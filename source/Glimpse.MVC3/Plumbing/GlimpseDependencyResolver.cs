using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Glimpse.Mvc3.Extensions;

namespace Glimpse.Mvc3.Plumbing
{
    //TODO:TEST ME
    internal class GlimpseDependencyResolver : IDependencyResolver
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


            Trace.Write(string.Format("IDependencyResolver.GetService<{0}>() = {1}", serviceType, service == null ? "null" : service.GetType().ToString()));
            return service;

        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var results = DependencyResolver.GetServices(serviceType);

            if (results.Count() > 0)
            {
                //TODO:Test the objects if needed here...
            }

            var resultString = "null";
            if (results.Count()!=0)
            {
                var stringBuilder = new StringBuilder();
                foreach (var result in results)
                {
                    stringBuilder.Append(result.GetType().ToString() + ",");
                }
                resultString = stringBuilder.ToString();
            }

            Trace.Write(string.Format("IDependencyResolver.GetServices<{0}>() = {1}", serviceType, resultString));
            return results;
        }
    }
}