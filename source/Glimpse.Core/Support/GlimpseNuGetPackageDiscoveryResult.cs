using System;
using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Support
{
    /// <summary>
    /// Represents the results of the <see cref="NuGetPackageAttribute"/> discovery process.
    /// </summary>
    public class GlimpseNuGetPackageDiscoveryResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseNuGetPackageDiscoveryResult"/> class.
        /// </summary>
        /// <param name="discoveredGlimpseNuGetPackages">List of discovered Glimpse NuGet packages</param>
        /// <param name="nonDiscoverableAssemblies">List of assemblies that were not accessible during discovery</param>
        public GlimpseNuGetPackageDiscoveryResult(NuGetPackageAttribute[] discoveredGlimpseNuGetPackages, Assembly[] nonDiscoverableAssemblies)
        {
            if (discoveredGlimpseNuGetPackages == null)
            {
                throw new ArgumentNullException("discoveredGlimpseNuGetPackages");
            }

            if (nonDiscoverableAssemblies == null)
            {
                throw new ArgumentNullException("nonDiscoverableAssemblies");
            }

            this.DiscoveredGlimpseNuGetPackages = discoveredGlimpseNuGetPackages;
            this.NonDiscoverableAssemblies = nonDiscoverableAssemblies;
        }

        /// <summary>
        /// Gets the list of discovered Glimpse NuGet packages
        /// </summary>
        public NuGetPackageAttribute[] DiscoveredGlimpseNuGetPackages { get; private set; }

        /// <summary>
        /// Gets the list of assemblies that were not accessible during discovery
        /// </summary>
        public Assembly[] NonDiscoverableAssemblies { get; private set; }
    }
}