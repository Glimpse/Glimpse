using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Extensions
{
    /// <summary>
    /// Extension methods to simplify common tasks completed with <see cref="IDataStore"/>.
    /// </summary>
    public static class DataStoreExtensions
    {
        /// <summary>
        /// Gets an item from <paramref name="store"/> using &lt;T&gt; as a key. This method should be used in combination with <c>Set&lt;T&gt;</c>.
        /// </summary>
        /// <typeparam name="T">The type of item to retrieve and the key when used in combination with <c>Set&lt;T&gt;</c>.</typeparam>
        /// <param name="store">The data store instance.</param>
        /// <returns>
        /// An item of type <typeparamref name="T"/>, <c>null</c> or an <see cref="InvalidCastException"/> is thrown depending on the type of <paramref name="store"/>. Call <c>Contains&lt;T&gt;</c> first to ensure results.
        /// </returns>
        /// <exception cref="InvalidCastException">May throw an invalid cast exception if the <see cref="IDataStore"/> item and type do not match.</exception>
        public static T Get<T>(this IDataStore store)
        {
            return (T)store.Get(KeyOf<T>());
        }

        /// <summary>
        /// Gets an item from <paramref name="store"/> using the key and casts to <c>&lt;T&gt;</c>.
        /// </summary>
        /// <typeparam name="T">The type of item to retrieve.</typeparam>
        /// <param name="store">The data store instance.</param>
        /// <param name="key">The item key.</param>
        /// <returns>
        /// An item of type <typeparamref name="T"/>, <c>null</c> or an <see cref="InvalidCastException"/> is thrown depending on the type of <paramref name="store"/>. Call <c>Contains&lt;T&gt;</c> first to ensure results.
        /// </returns>
        /// <exception cref="InvalidCastException">May throw an invalid cast exception if the <see cref="IDataStore"/> item and type do not match.</exception>
        public static T Get<T>(this IDataStore store, string key)
        {
            return (T)store.Get(key);
        }

        /// <summary>
        /// Sets an item in <paramref name="store"/> using &lt;T&gt; as a key. This method should be used in combination with <c>Get&lt;T&gt;</c>.
        /// </summary>
        /// <typeparam name="T">The type of item to set.</typeparam>
        /// <param name="store">The data store instance.</param>
        /// <param name="value">The item to store.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// dataStore.Set("I am a string"); // inferred type
        /// var result = dataStore.Get<string>();
        /// Assert.Equals(result, "I am a string");
        /// 
        /// dataStore.Set<IList<int>>(new List{ 1, 2, 3 }); // specified type
        /// var result = dataStore.Get<IList<int>>();
        /// Assert.Equals(result.Count, 3);
        /// ]]>
        /// </code>
        /// </example>
        public static void Set<T>(this IDataStore store, T value)
        {
            store.Set(KeyOf<T>(), value);
        }

        /// <summary>
        /// Determines whether <paramref name="store"/> contains and item with a key of <c>T</c>.
        /// </summary>
        /// <typeparam name="T">The item key.</typeparam>
        /// <param name="store">The data store instance.</param>
        /// <returns>
        ///   <c>true</c> if store contains the item; otherwise, <c>false</c>.
        /// </returns>
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
