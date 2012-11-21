namespace Glimpse.Core.Extensibility
{
    public interface IDataStore
    {
        T Get<T>();
        
        T Get<T>(string key);
        
        object Get(string key);

        void Set<T>(T value);
        
        void Set(string key, object value);

        bool Contains<T>();

        bool Contains(string key);
    }
}
