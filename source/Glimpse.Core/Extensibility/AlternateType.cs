using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Extensibility
{
    public abstract class AlternateType<T> where T : class
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

        public virtual bool TryCreate(T originalObj, out T newObj, IEnumerable<object> mixins = null, object[] constructorArguments = null)
        {
            var objType = originalObj.GetType();
            var allMethods = AllMethods();

            if (mixins == null)
            {
                mixins = Enumerable.Empty<object>();
            }

            if (ProxyFactory.IsWrapInterfaceEligible(objType))
            {
                newObj = ProxyFactory.WrapInterface(originalObj, allMethods, mixins);
                return true;
            }

            if (ProxyFactory.IsWrapClassEligible(objType))
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

        public abstract IEnumerable<IAlternateMethod> AllMethods();
    }
}
