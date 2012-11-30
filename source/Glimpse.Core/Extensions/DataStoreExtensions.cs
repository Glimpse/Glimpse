using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Extensions
{
    public static class DataStoreExtensions
    {
        public static T Get<T>(this IDataStore store)
        {
            return (T)store.Get(KeyOf<T>());
        }

        public static T Get<T>(this IDataStore store, string key)
        {
            return (T)store.Get(key);
        }

        public static void Set<T>(this IDataStore store, T value)
        {
            store.Set(KeyOf<T>(), value);
        }

        public static bool Contains<T>(this IDataStore store)
        {
            return store.Contains(KeyOf<T>());
        }

        private static string KeyOf<T>()
        {
            return typeof(T).AssemblyQualifiedName;
        }
    }
}
