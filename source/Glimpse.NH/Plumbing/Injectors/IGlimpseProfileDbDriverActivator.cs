using System;
using Glimpse.NH.Plumbing.Profiler;

namespace Glimpse.NH.Plumbing.Injectors
{
    public interface IGlimpseProfileDbDriverActivator
    {
        IGlimpseProfileDbDriver CreateProfileDbDriver(Type profileDbDriverType);
    }
}