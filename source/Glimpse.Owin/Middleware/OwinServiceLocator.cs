using System.Collections.Generic;
using Glimpse.Core.Framework;
using Owin;

namespace Glimpse.Owin.Middleware
{
    public class OwinServiceLocator : IServiceLocator
    {
        private readonly IDictionary<string, object> environment;
        private readonly IAppBuilder app;

        public OwinServiceLocator(IDictionary<string, object> environment, IAppBuilder app)
        {
            this.environment = environment;
            this.app = app;
        }

        public T GetInstance<T>() where T : class
        {
            if (typeof(T) == typeof(IFrameworkProvider))
            {
                return new OwinFrameworkProvider(environment, app) as T;
            }

            return null;
        }

        public ICollection<T> GetAllInstances<T>() where T : class
        {
            return null;
        }
    }
}