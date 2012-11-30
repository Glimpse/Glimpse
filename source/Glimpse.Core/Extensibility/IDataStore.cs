namespace Glimpse.Core.Extensibility
{
    public interface IDataStore
    {
        object Get(string key);
        
        void Set(string key, object value);

        bool Contains(string key);
    }
}
