using System;
using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    public abstract class Alternate<T> where T : class
    {
        protected Alternate(IProxyFactory proxyFactory)
        {
            if (proxyFactory == null)
            {
                throw new ArgumentNullException("proxyFactory");
            }

            ProxyFactory = proxyFactory;
        }

        public IProxyFactory ProxyFactory { get; set; }

        public virtual bool TryCreate(T originalObj, out T newObj)
        {
            return TryCreate(originalObj, out newObj, null);
        }

        public virtual bool TryCreate(T originalObj, out T newObj, object mixin)
        {
            if (ProxyFactory.IsProxyable(originalObj))
            {
                newObj = ProxyFactory.CreateProxy(originalObj, AllMethods(), mixin);
                return true;
            }

            newObj = null;
            return false;
        }

        public abstract IEnumerable<IAlternateImplementation<T>> AllMethods();
    }
}
