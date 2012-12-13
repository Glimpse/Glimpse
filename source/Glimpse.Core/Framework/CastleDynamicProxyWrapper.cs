using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class CastleDynamicProxyWrapper<T> : IWrapper<T>
    {
        internal IProxyTargetAccessor ProxyTargetAccessor { get; set; }

        public T GetWrappedObject()
        {
            if (ProxyTargetAccessor == null)
            {
                return default(T);
            }

            return (T)ProxyTargetAccessor.DynProxyGetTarget();
        }
    }
}