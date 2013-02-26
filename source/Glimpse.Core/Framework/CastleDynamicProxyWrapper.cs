using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// An implementation of <see cref="IWrapper{T}"/> used by <see cref="CastleDynamicProxyFactory"/> to provide mixin support.
    /// </summary>
    /// <typeparam name="T">The type being wrapped.</typeparam>
    internal class CastleDynamicProxyWrapper<T> : IWrapper<T>
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