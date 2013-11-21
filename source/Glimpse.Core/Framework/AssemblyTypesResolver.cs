using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glimpse.Core.Framework
{
    public static class AssemblyTypesResolver
    {
        private static readonly Dictionary<Assembly, Type[]> typesByAssembly = new Dictionary<Assembly, Type[]>();

        public static IEnumerable<Type> RetrieveTypes(Assembly assembly)
        {
            // GetTypes potentially throws an exception. Defensive coding as per http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx
            
            Type[] allTypes; lock (((IDictionary)typesByAssembly).SyncRoot)
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
                        GlimpseConfiguration.GetLogger().Warn(string.Format(Resources.DiscoverGetType, assembly.FullName), exception);
                    }
                }
            }
            return allTypes;
        }
    }
}