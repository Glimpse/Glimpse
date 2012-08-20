using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    public interface IProxyFactory
    {
        bool IsProxyable(object obj);
        T CreateProxy<T>(T instance, IEnumerable<IAlternateImplementation<T>> methodImplementations) where T : class;
        T CreateProxy<T>(T instance, IEnumerable<IAlternateImplementation<T>> methodImplementations, object mixin) where T : class;
    }
}