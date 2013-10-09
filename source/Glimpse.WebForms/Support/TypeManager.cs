using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glimpse.WebForms.Support
{
    public class TypeManager
    {
        private static IEnumerable<Type> allTypes;
        private static IDictionary<string, Type> matchedTypes = new Dictionary<string, Type>();
        private static object lockTypes = new object(); 

        public static Type TryDeriveType(string type)
        {
            Type result;
            if (!matchedTypes.TryGetValue(type, out result))
            {
                lock (lockTypes)
                {
                    if (!matchedTypes.TryGetValue(type, out result))
                    {
                        if (allTypes == null)
                        {
                            allTypes = GetAllTypes(); 
                        }

                        result = allTypes.FirstOrDefault(x => x.FullName == type);
                        matchedTypes.Add(type, result);
                    }
                }
            }

            return result;
        }

        private static IEnumerable<Type> GetAllTypes()
        {
            var results = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) 
            {
                try
                {
                    Type[] allTypes;
                    try
                    {
                        allTypes = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        allTypes = ex.Types.Where(t => t != null).ToArray();
                    }

                    results.AddRange(allTypes);
                }
                catch (Exception exception)
                { 
                }
            }

            return results;
        } 
    }
}