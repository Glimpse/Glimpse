using System;
using Glimpse.NH.Plumbing.Profiler;

namespace Glimpse.NH.Plumbing.Injectors
{
    public class GlimpseProfileDbDriverActivator : IGlimpseProfileDbDriverActivator
    {
        public IGlimpseProfileDbDriver CreateProfileDbDriver(Type profileDbDriverType)
        {
            if (profileDbDriverType == null) 
                throw new ArgumentNullException("profileDbDriverType");

            var profileDbConstructor = profileDbDriverType.GetConstructor(Type.EmptyTypes);
            if (profileDbConstructor == null)
                throw new InvalidOperationException(string.Format("{0} should have a parameterless constructor", profileDbDriverType));

            var profileDbDriver = (IGlimpseProfileDbDriver) profileDbConstructor.Invoke(null);
            return profileDbDriver;
        }
    }
}