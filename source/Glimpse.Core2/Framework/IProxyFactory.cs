using System.Collections.Generic;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public interface IProxyFactory
    {
        T CreateProxy<T>(T instance, IEnumerable<IAlternateMethodImplementation<T>> methodImplementations) where T : class;
    }
}