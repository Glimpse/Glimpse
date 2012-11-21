using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet
{
    public class HttpApplicationStateBaseDataStoreAdapter : IDataStore
    {
        public HttpApplicationStateBaseDataStoreAdapter(HttpApplicationStateBase applicationState)
        {
            ApplicationState = applicationState;
        }

        internal HttpApplicationStateBase ApplicationState { get; set; }

        public T Get<T>()
        {
            return Get<T>(KeyOf<T>());
        }

        public T Get<T>(string key)
        {
            return (T)Get(key);
        }

        public object Get(string key)
        {
            return ApplicationState.Get(key);
        }

        public void Set<T>(T value)
        {
            Set(KeyOf<T>(), value);
        }

        public void Set(string key, object value)
        {
            ApplicationState.Set(key, value);
        }

        public bool Contains<T>()
        {
            return Contains(KeyOf<T>());
        }

        public bool Contains(string key)
        {
            var result = ApplicationState.Get(key);

            return result != null;
        }

        private static string KeyOf<T>()
        {
            return typeof(T).AssemblyQualifiedName;
        }
    }
}
