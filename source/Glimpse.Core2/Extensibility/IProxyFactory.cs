using System.Collections.Generic;

namespace Glimpse.Core2.Extensibility
{
    public interface IProxyFactory
    {
        T CreateProxy<T>(T instance, IEnumerable<IAlternateImplementation<T>> methodImplementations) where T : class;
    }
}