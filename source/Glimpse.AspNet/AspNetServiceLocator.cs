using System.Collections.Generic;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class AspNetServiceLocator : IServiceLocator
    {
        public T GetInstance<T>() where T : class
        {
            var type = typeof(T);
            if (type == typeof(IFrameworkProvider))
            {
                return new AspNetFrameworkProvider() as T;
            }

            if (type == typeof(ResourceEndpointConfiguration))
            {
                return new HttpHandlerEndpointConfiguration() as T;
            }

            return null;
        }

        public ICollection<T> GetAllInstances<T>() where T : class
        {
            return null;
        }
    }
}