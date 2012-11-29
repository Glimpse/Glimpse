using System;
using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    public interface IProxyFactory
    {
        bool IsWrapInterfaceEligible(Type type);

        T WrapInterface<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations) where T : class;

        T WrapInterface<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class;

        bool IsWrapClassEligible(Type type);

        T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations) where T : class;

        T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class;

        T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins, IEnumerable<object> constructorArguments) where T : class;

        bool IsExtendClassEligible(Type type);

        T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations) where T : class;

        T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class;

        T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins, IEnumerable<object> constructorArguments) where T : class;
    }
}