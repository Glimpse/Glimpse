using System;
using System.Collections.Generic;
using System.Linq; 
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Support
{
    public static class NuGetPackage
    {
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

                var version = nugetPackage.GetVersion();
                var id = nugetPackage.GetId();

                packages[id] = version;
            }

            return packages;
        }

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

                packages.Add(nugetPackage);
            }

            return packages;
        }
    }
}
