using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Support
{
    /// <summary>
    /// Discoverer for assemblies defined as Glimpse NuGet packages
    /// </summary>
    public class GlimpseNuGetPackageDiscoverer
    {
        /// <summary>
        /// Discovers Glimpse NuGet packages by checking all loaded assemblies for the presence of the <see cref="NuGetPackageAttribute"/> attribute,
        /// while keeping track of assemblies that were not accessible during discovery, if any.
        /// </summary>
        /// <returns>The result of the discovery.</returns>
        public static GlimpseNuGetPackageDiscoveryResult Discover()
        {
            var discoveredGlimpseNuGetPackages = new List<NuGetPackageAttribute>();
            var nonDiscoverableAssemblies = new List<Assembly>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                NuGetPackageAttribute nuGetPackageAttribute = null;

                try
                {
                    nuGetPackageAttribute = assembly.GetCustomAttributes(typeof(NuGetPackageAttribute), false).Cast<NuGetPackageAttribute>().SingleOrDefault();
                }
                catch (Exception exception)
                {
#warning need to find a better way to get to the logger
                    GlimpseConfiguration.GetLogger().Error(string.Format("Failed requesting custom attributes of assembly '{0}'", assembly.FullName), exception);
                    nonDiscoverableAssemblies.Add(assembly);
                }

                if (nuGetPackageAttribute != null)
                {
                    nuGetPackageAttribute.Initialize(assembly);
                    discoveredGlimpseNuGetPackages.Add(nuGetPackageAttribute);
                }
            }

            return new GlimpseNuGetPackageDiscoveryResult(discoveredGlimpseNuGetPackages.ToArray(), nonDiscoverableAssemblies.ToArray());
        }
    }
}