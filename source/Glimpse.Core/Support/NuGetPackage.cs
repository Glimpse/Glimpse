using System;
using System.Collections.Generic;
using System.Linq; 
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Support
{
    /// <summary>
    /// Helper methods designed to aid in the discovery of which
    /// Glimpse nuget packages are currently installed.
    /// </summary>
    public static class NuGetPackage
    {
        /// <summary>
        /// Gets the registered package names and versions.
        /// </summary>
        /// <returns>Found entries.</returns>
        public static IDictionary<string, string> GetRegisteredPackageVersions()
        {
            var packages = new Dictionary<string, string>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var nugetPackage = assembly.GetCustomAttributes(typeof(NuGetPackageAttribute), false).Cast<NuGetPackageAttribute>().SingleOrDefault();
                if (nugetPackage == null)
                {
                    continue;
                }

                nugetPackage.Initialize(assembly);

                var version = nugetPackage.GetVersion();
                var id = nugetPackage.GetId();

                packages[id] = version;
            }

            return packages;
        }

        /// <summary>
        /// Gets the registered packages attribute registration.
        /// </summary>
        /// <returns>Found entries.</returns>
        public static IList<NuGetPackageAttribute> GetRegisteredPackages()
        {
            var packages = new List<NuGetPackageAttribute>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var nugetPackage = assembly.GetCustomAttributes(typeof(NuGetPackageAttribute), false).Cast<NuGetPackageAttribute>().SingleOrDefault();
                if (nugetPackage == null)
                {
                    continue;
                }

                nugetPackage.Initialize(assembly);

                packages.Add(nugetPackage);
            }

            return packages;
        }
    }
}
