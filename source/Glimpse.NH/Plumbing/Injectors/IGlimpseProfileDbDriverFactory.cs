using System;
using System.Reflection;

namespace Glimpse.NH.Plumbing.Injectors
{
    public interface IGlimpseProfileDbDriverFactory
    {
        Type GetProfileDbDriverType(Assembly nhibernateAssembly);
    }
}