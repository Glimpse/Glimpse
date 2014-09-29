using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// A discoverer that finds and loads types by means of .NET reflection
    /// </summary>
    public class ReflectionBasedTypeDiscoverer
    {
        private static string ApplicationBaseDirectory { get; set; }

        private ILogger Logger { get; set; }

        static ReflectionBasedTypeDiscoverer()
        {
            // Initializes the base directory of the application, if the AppDomain is shadow copied, use the shadow directory
            var setupInfo = AppDomain.CurrentDomain.SetupInformation;
            ApplicationBaseDirectory = string.Equals(setupInfo.ShadowCopyFiles, "true", StringComparison.OrdinalIgnoreCase)
                       ? Path.Combine(setupInfo.CachePath, setupInfo.ApplicationName)
                       : AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionBasedTypeDiscoverer" /> class
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ReflectionBasedTypeDiscoverer(ILogger logger)
        {
            Logger = logger;
        }

        public TInstance[] ResolveAndCreateInstancesOfType<TInstance>(string discoveryLocation, Type[] typesToIgnore)
        {
            discoveryLocation = SanitizeDiscoveryLocation(discoveryLocation);

            Logger.Debug("Discovering {0}'s in '{1}' and all sub directories.", typeof(TInstance).Name, discoveryLocation);

            var resolvedInstances = new List<TInstance>();

            resolvedInstances.AddRange(discoveryLocation.Equals(ApplicationBaseDirectory)
                ? GetInstancesOfType<TInstance>(typesToIgnore)
                : GetInstancesOfType<TInstance>(discoveryLocation, typesToIgnore));

            return resolvedInstances.ToArray();
        }

        private static string SanitizeDiscoveryLocation(string discoveryLocation)
        {
            var sanitizedDiscoveryLocation = discoveryLocation ?? ApplicationBaseDirectory;

            // If this isn't an absolute path then root it with the Application base directory
            sanitizedDiscoveryLocation = Path.IsPathRooted(sanitizedDiscoveryLocation) ? sanitizedDiscoveryLocation : Path.Combine(ApplicationBaseDirectory, sanitizedDiscoveryLocation);

            if (!Directory.Exists(sanitizedDiscoveryLocation))
            {
                throw new DirectoryNotFoundException(string.Format(Resources.SetDiscoveryLocationDirectoryNotFoundMessage, discoveryLocation, sanitizedDiscoveryLocation));
            }

            return sanitizedDiscoveryLocation;
        }

        private IEnumerable<TInstance> GetInstancesOfType<TInstance>(Type[] typesToIgnore)
        {
            var instances = new List<TInstance>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    instances.AddRange(GetInstancesOfType<TInstance>(assembly, typesToIgnore));
                }
                catch (Exception exception)
                {
                    string assemblyLocation;
                    try
                    {
                        assemblyLocation = assembly.Location;
                    }
                    catch (NotSupportedException)
                    {
                        assemblyLocation = "in-memory";
                    }

                    Logger.Error(Resources.DiscoverLoadAssembly, exception, assemblyLocation);
                }
            }

            return instances;
        }

        private IEnumerable<TInstance> GetInstancesOfType<TInstance>(string discoveryLocation, Type[] typesToIgnore)
        {
            var instances = new List<TInstance>();

            foreach (var file in Directory.GetFiles(discoveryLocation, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    instances.AddRange(GetInstancesOfType<TInstance>(assembly, typesToIgnore));
                }
                catch (Exception exception)
                {
                    Logger.Error(Resources.DiscoverLoadAssembly, exception, file);
                }
            }

            return instances;
        }

        private IEnumerable<TInstance> GetInstancesOfType<TInstance>(Assembly assembly, Type[] typesToIgnore)
        {
            if (ReflectionBlackList.IsBlackListed(assembly))
            {
                return Enumerable.Empty<TInstance>();
            }

            var allTypes = AssemblyTypesResolver.ResolveTypes(assembly, Logger);

            var matchingTypes = allTypes.Where(type => !typesToIgnore.Contains(type) &&
                                                       !type.IsInterface &&
                                                       !type.IsAbstract &&
                                                        typeof(TInstance).IsAssignableFrom(type));

            var instances = new List<TInstance>();

            foreach (var matchingType in matchingTypes)
            {
                try
                {
                    var instance = (TInstance)Activator.CreateInstance(matchingType);
                    instances.Add(instance);
                }
                catch (Exception exception)
                {
                    Logger.Error(Resources.DiscoverCreateInstance, exception, typeof(TInstance), matchingType);
                }
            }

            return instances;
        }
    }
}