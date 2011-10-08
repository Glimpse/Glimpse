using System.Collections;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet
{
    public class DictionaryDataStoreAdapter:IDataStore
    {
        internal IDictionary Dictionary { get; set; }

        public DictionaryDataStoreAdapter(IDictionary dictionary)
        {
            Dictionary = dictionary;
        }

        public T Get<T>()
        {
            return (T) Dictionary[typeof(T)];
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
            Dictionary[typeof (T)] = value;
        }

        public void Set(string key, object value)
        {
            Dictionary[key] = value;
        }
    }
}
