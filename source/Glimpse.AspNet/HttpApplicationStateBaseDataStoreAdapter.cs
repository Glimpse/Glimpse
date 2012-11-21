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
            return Get<T>(typeof(T).AssemblyQualifiedName);
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
            ApplicationState.Set(typeof(T).AssemblyQualifiedName, value);
        }

        public void Set(string key, object value)
        {
            ApplicationState.Set(key, value);
        }

        public bool Contains(string key)
        {
            var result = ApplicationState.Get(key);

            return result != null;
        }
    }
}
