using System;
using System.Collections;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An adapter between an <see cref="IDictionary"/> and <see cref="IDataStore"/>.
    /// </summary>
    public class DictionaryDataStoreAdapter : IDataStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryDataStoreAdapter" /> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <exception cref="System.ArgumentException">Throws an exception if a generic dictionary does not have a value of type <see cref="object"/> and a key of either <see cref="string"/> or <see cref="object"/>.</exception>
        public DictionaryDataStoreAdapter(IDictionary dictionary)
        {
            if (!IsValidDictionaryType(dictionary))
            {
                throw new ArgumentException("dictionary");
            }
            
            Dictionary = dictionary;
        }

        /// <summary>
        /// Gets or sets the dictionary.
        /// </summary>
        /// <value>
        /// The dictionary.
        /// </value>
        internal IDictionary Dictionary { get; set; }

        /// <summary>
        /// Gets the item with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value stored at given key.
        /// </returns>
        public object Get(string key)
        {
            return Dictionary[key];
        }

        /// <summary>
        /// Sets the item with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string key, object value)
        {
            Dictionary[key] = value;
        }

        /// <summary>
        /// Determines whether the data store contains a definition for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if it contains the specified key; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string key)
        {
            return Dictionary.Contains(key);
        }

        private static bool IsValidDictionaryType(IDictionary dictionary)
        {
            if (dictionary == null)
            {
                return false;
            }

            Type[] genericParameters = dictionary.GetType().GetGenericArguments();

            // Support non-generics IDictionary
            if (genericParameters.Length == 0)
            {
                return true;
            }

            // Only support IDictionary<string|object, object>
            if (genericParameters[0] != typeof(string) && genericParameters[0] != typeof(object))
            {
                return false;
            }

            if (genericParameters[1] != typeof(object))
            {
                return false;
            }

            return true;
        }
    }
}