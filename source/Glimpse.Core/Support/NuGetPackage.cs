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

                var version = nugetPackage.GetVersion(assembly);
                var id = nugetPackage.GetId(assembly);

                packages[id] = version;
            }

            return packages;
        }
    }
}
