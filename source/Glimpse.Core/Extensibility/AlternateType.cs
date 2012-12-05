using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Extensibility
{
    public abstract class AlternateType<T> : IAlternateType<T> where T : class
    {
        protected AlternateType(IProxyFactory proxyFactory)
        {
            if (proxyFactory == null)
            {
                throw new ArgumentNullException("proxyFactory");
            }

            ProxyFactory = proxyFactory;
        }

        public IProxyFactory ProxyFactory { get; set; }

        public abstract IEnumerable<IAlternateMethod> AllMethods { get; }

        public virtual bool TryCreate(T originalObj, out T newObj)
        {
            return TryCreate(originalObj, out newObj, null, null);
        }

        public virtual bool TryCreate(T originalObj, out T newObj, IEnumerable<object> mixins)
        {
            return TryCreate(originalObj, out newObj, mixins, null);
        }

        public virtual bool TryCreate(T originalObj, out T newObj, IEnumerable<object> mixins, object[] constructorArguments)
        {
            var allMethods = AllMethods;

            if (mixins == null)
            {
                mixins = Enumerable.Empty<object>();
            }

            if (ProxyFactory.IsWrapInterfaceEligible<T>(typeof(T)))
            {
                newObj = ProxyFactory.WrapInterface(originalObj, allMethods, mixins);
                return true;
            }

            if (ProxyFactory.IsWrapClassEligible(typeof(T)))
            {
                try
                {
                    newObj = ProxyFactory.WrapClass(originalObj, allMethods, mixins, constructorArguments);
                    return true;
                }
                catch
                {
                    newObj = null;
                    return false;
                }
            }

            newObj = null;
            return false;
        }
    }
}
