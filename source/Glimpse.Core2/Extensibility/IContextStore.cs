namespace Glimpse.Core2.Extensibility
{
    public interface IContextStore
    {
        T Get<T>();
        T Get<T>(string key);
        object Get(string key);

        void Set<T>();
        void Set(string key);
    }
}
