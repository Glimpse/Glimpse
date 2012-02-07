using System.Collections.Generic;
using Glimpse.Core2.Framework;

namespace Glimpse.AspNet
{
    public class AspNetServiceLocator:IServiceLocator
    {
        public T GetInstance<T>() where T:class
        {
            var tType = typeof (T);
            if (tType == typeof(IFrameworkProvider))
                return new AspNetFrameworkProvider() as T;

            if (tType == typeof(ResourceEndpointConfiguration))
                return new HttpHandlerEndpointConfiguration() as T;

            return null;
        }

        public ICollection<T> GetAllInstances<T>() where T : class
        {
            return null;
        }
    }
}