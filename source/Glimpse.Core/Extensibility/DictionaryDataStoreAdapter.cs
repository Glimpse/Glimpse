using System;
using System.Collections;

namespace Glimpse.Core.Extensibility
{
    public class DictionaryDataStoreAdapter : IDataStore
    {
        public DictionaryDataStoreAdapter(IDictionary dictionary)
        {
            if (!IsValidDictionaryType(dictionary))
            {
                throw new ArgumentException("dictionary");
            }
            
            Dictionary = dictionary;
        }

        internal IDictionary Dictionary { get; set; }

        public object Get(string key)
        {
            return Dictionary[key];
        }

        public void Set(string key, object value)
        {
            Dictionary[key] = value;
        }

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