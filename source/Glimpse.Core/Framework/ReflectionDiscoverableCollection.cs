using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class ReflectionDiscoverableCollection<T> : IDiscoverableCollection<T>
    {
        private string discoveryLocation;

        public ReflectionDiscoverableCollection(ILogger logger)
        {
            Items = new List<T>();
            IgnoredTypes = new List<Type>();
            AutoDiscover = true;
            Logger = logger;
        }

        public bool AutoDiscover { get; set; }

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public string DiscoveryLocation
        {
            get
            {
                return discoveryLocation ?? (discoveryLocation = BaseDirectory);
            }

            set 
            {
                // If this isn't an absolute path then root it with the AppDomain's base directory
                var result = Path.IsPathRooted(value) ? value : Path.Combine(BaseDirectory, value);

                if (!Directory.Exists(result))
                {
                    throw new DirectoryNotFoundException(string.Format(Resources.SetDiscoveryLocationDirectoryNotFoundMessage, value, result));
                }
                
                discoveryLocation = result;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        internal List<T> Items { get; set; }

        internal List<Type> IgnoredTypes { get; set; }

        internal ILogger Logger { get; set; }

        // Get the directory of the application, if the AppDomain is shadow copied, use the shadow directory
        private static string BaseDirectory 
        {
            get 
            {
                var setupInfo = AppDomain.CurrentDomain.SetupInformation;
                return string.Equals(setupInfo.ShadowCopyFiles, "true", StringComparison.OrdinalIgnoreCase)
                           ? Path.Combine(setupInfo.CachePath, setupInfo.ApplicationName)
                           : AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public void Add(T item)
        {
            Items.Add(item);
            Logger.Debug(Resources.DiscoverableCollectionAdd, typeof(T).Name, item.GetType());
        }

        public void Clear()
        {
            Items.Clear();
            Logger.Debug(Resources.DiscoverableCollectionClear, typeof(T).Name);
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var result = Items.Remove(item);

            if (result)
            {
                Logger.Debug(Resources.DiscoverableCollectionRemove, typeof(T).Name, item.GetType());
            }

            return result;
        }

        public void IgnoreType(Type type)
        {
            IgnoredTypes.Add(type);
        }

        public void Discover()
        {
            Logger.Debug("Discovering {0}'s in '{1}' and all subdirectories.", typeof(T).Name, DiscoveryLocation);

            var results = new List<T>();

            foreach (var file in Directory.GetFiles(DiscoveryLocation, "*.dll", SearchOption.AllDirectories))
            {
                try 
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    Type[] allTypes;

                    // GetTypes potentially throws and exception. Defensive coding as per http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx
                    try
                    {
                        allTypes = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        allTypes = ex.Types.Where(t => t != null).ToArray();

                        foreach (var exception in ex.LoaderExceptions)
                        {
                            Logger.Warn(Resources.DiscoverGetType, exception);
                        }
                    }
                    
                    var concreteTypes = allTypes.Where(type => typeof(T).IsAssignableFrom(type) &&
                                                                          !type.IsInterface &&
                                                                          !type.IsAbstract &&
                                                                          !IgnoredTypes.Contains(type));
                    foreach (var type in concreteTypes)
                    {
                        try
                        {
                            var instance = (T)Activator.CreateInstance(type);
                            results.Add(instance);
                        }
                        catch (Exception exception)
                        {
                            Logger.Error(Resources.DiscoverCreateInstance, exception, typeof(T), type);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logger.Error(Resources.DiscoverLoadAssembly, exception, file);
                }
            }

            if (results.Count > 0)
            {
                Items.Clear();

                foreach (var result in results)
                {
                    Logger.Debug(string.Format(Resources.DiscoverableCollectionDiscover, typeof(T).Name, result.GetType()));
                }

                Items.AddRange(results);
            }
        }
    }
}