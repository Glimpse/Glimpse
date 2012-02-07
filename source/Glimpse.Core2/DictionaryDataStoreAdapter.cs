using System;
using System.Collections;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2
{
    public class DictionaryDataStoreAdapter : IDataStore
    {
        public DictionaryDataStoreAdapter(IDictionary dictionary)
        {
            Contract.Requires<ArgumentException>(IsValidDictionaryType(dictionary), "dictionary");

            Dictionary = dictionary;
        }

        internal IDictionary Dictionary { get; set; }

        public T Get<T>()
        {
            return (T) Dictionary[typeof (T).AssemblyQualifiedName];
        }

        public T Get<T>(string key)
        {
            return (T) Dictionary[key];
        }

        public object Get(string key)
        {
            return Dictionary[key];
        }

        public void Set<T>(T value)
        {
            Dictionary[typeof (T).AssemblyQualifiedName] = value;
        }

        public void Set(string key, object value)
        {
            Dictionary[key] = value;
        }

        public bool Contains(string key)
        {
            return Dictionary.Contains(key);
        }

        [Pure]
        public static bool IsValidDictionaryType(IDictionary dictionary)
        {
            Type[] genericParameters = dictionary.GetType().GetGenericArguments();

            //Support non-generics IDictionary
            if (genericParameters.Length == 0) return true;
            //Only support IDictionary<string|object, object>
            if (genericParameters[0] != typeof (string) && genericParameters[0] != typeof (object)) return false;
            if (genericParameters[1] != typeof (object)) return false;

            return true;
        }
    }
}