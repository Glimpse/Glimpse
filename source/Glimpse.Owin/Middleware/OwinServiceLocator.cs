using System.Collections.Generic;
using Glimpse.Core.Framework;

namespace Glimpse.Owin.Middleware
{
    public class OwinServiceLocator : IServiceLocator
    {
        private readonly IDictionary<string, object> environment;
        private readonly IDictionary<string, object> serverStore;

        public OwinServiceLocator(IDictionary<string, object> environment, IDictionary<string, object> serverStore)
        {
            this.environment = environment;
            this.serverStore = serverStore;
        }

        public T GetInstance<T>() where T : class
        {
            if (typeof(T) == typeof(IFrameworkProvider))
            {
                return new OwinFrameworkProvider(environment, serverStore) as T;
            }

            return null;
        }

        public ICollection<T> GetAllInstances<T>() where T : class
        {
            return null;
        }
    }
}