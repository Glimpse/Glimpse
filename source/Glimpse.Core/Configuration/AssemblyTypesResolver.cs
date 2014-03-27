using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Resolves type for requested assemblies and caches the results for later use
    /// </summary>
    public static class AssemblyTypesResolver
    {
        private static readonly Dictionary<Assembly, Type[]> typesByAssembly = new Dictionary<Assembly, Type[]>();

        /// <summary>
        /// Resolves the types for the given assembly
        /// </summary>
        /// <remarks>
        /// The resolver will first check its cache. If the types were previously resolved for the given assembly, then those are returned
        /// If nothing has been cached before, then the types are being resolved and added to the cache before being returned.
        /// </remarks>
        /// <param name="assembly">The assembly for which the types should be resolved</param>
        /// <param name="logger">The logger to use in case an exception should occur when resolving the types</param>
        /// <returns>List of types resolved for the given assembly</returns>
        public static Type[] ResolveTypes(Assembly assembly, ILogger logger)
        {
            // GetTypes potentially throws an exception. Defensive coding as per http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx
            Type[] allTypes;
            lock (typesByAssembly)
            {
                try
                {
                    if (!typesByAssembly.TryGetValue(assembly, out allTypes))
                    {
                        allTypes = assembly.GetTypes();
                        typesByAssembly.Add(assembly, allTypes);
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    allTypes = ex.Types.Where(t => t != null).ToArray();
                    typesByAssembly.Add(assembly, allTypes);

                    foreach (var exception in ex.LoaderExceptions)
                    {
                        logger.Warn(string.Format(Resources.DiscoverGetType, assembly.FullName), exception);
                    }
                }
            }

            return allTypes;
        }
    }
}