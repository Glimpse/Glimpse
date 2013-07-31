using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Support
{
    /// <summary>
    /// Discoverer for assemblies defined as NuGet packages by means of the <see cref="NuGetPackageAttribute"/>
    /// </summary>
    public class NuGetPackageDiscoverer
    {
        /// <summary>
        /// Discovers NuGet packages by checking all loaded assemblies for the presence of the <see cref="NuGetPackageAttribute"/> attribute,
        /// while keeping track of assemblies that could not be processed during discovery, if any.
        /// </summary>
        /// <returns>The result of the discovery.</returns>
        public static NuGetPackageDiscoveryResult Discover()
        {
            var foundNuGetPackages = new List<NuGetPackageAttribute>();
            var nonProcessableAssemblies = new List<Assembly>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                NuGetPackageAttribute nuGetPackageAttribute = null;

                try
                {
                    nuGetPackageAttribute = assembly.GetCustomAttributes(typeof(NuGetPackageAttribute), false)
                                                    .Cast<NuGetPackageAttribute>()
                                                    .SingleOrDefault();
                }
                catch (Exception exception)
                {
                    GlimpseConfiguration.GetLogger().Error(string.Format("Failed requesting custom attributes of assembly '{0}'", assembly.FullName), exception);
                    nonProcessableAssemblies.Add(assembly);
                }

                if (nuGetPackageAttribute != null)
                {
                    nuGetPackageAttribute.Initialize(assembly);
                    foundNuGetPackages.Add(nuGetPackageAttribute);
                }
            }

            return new NuGetPackageDiscoveryResult(foundNuGetPackages.ToArray(), nonProcessableAssemblies.ToArray());
        }
    }
}