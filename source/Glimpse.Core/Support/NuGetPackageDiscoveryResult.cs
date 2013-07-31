using System;
using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Support
{
    /// <summary>
    /// Represents the results of discovering NuGet packages based on the <see cref="NuGetPackageAttribute"/>.
    /// </summary>
    public class NuGetPackageDiscoveryResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageDiscoveryResult"/> class.
        /// </summary>
        /// <param name="foundNuGetPackages">List of found NuGet packages</param>
        /// <param name="nonProcessableAssemblies">List of assemblies that could not be processed during discovery</param>
        public NuGetPackageDiscoveryResult(NuGetPackageAttribute[] foundNuGetPackages, Assembly[] nonProcessableAssemblies)
        {
            if (foundNuGetPackages == null)
            {
                throw new ArgumentNullException("foundNuGetPackages");
            }

            if (nonProcessableAssemblies == null)
            {
                throw new ArgumentNullException("nonProcessableAssemblies");
            }

            this.FoundNuGetPackages = foundNuGetPackages;
            this.NonProcessableAssemblies = nonProcessableAssemblies;
        }

        /// <summary>
        /// Gets the list of found NuGet packages
        /// </summary>
        public NuGetPackageAttribute[] FoundNuGetPackages { get; private set; }

        /// <summary>
        /// Gets the list of assemblies that could npt be processed during discovery
        /// </summary>
        public Assembly[] NonProcessableAssemblies { get; private set; }
    }
}