using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Framework
{
    internal class BaseDataProvider : BaseProvider
    {
        public BaseDataProvider(IReadonlyConfiguration configuration, ActiveGlimpseRequestContexts activeGlimpseRequestContexts)
            : base(configuration, activeGlimpseRequestContexts)
        {
        }

        protected TResult GetResultsStore<TResult>(IGlimpseRequestContext glimpseRequestContext, string resultStoreKey)
            where TResult : class, new()
        {
            var requestStore = glimpseRequestContext.RequestStore;

            var resultStore = requestStore.Get<TResult>(resultStoreKey);
            if (resultStore == null)
            {
                resultStore = new TResult();
                requestStore.Set(resultStoreKey, resultStore);
            }

            return resultStore;
        }

        protected IDataStore GetTabStore(string tabName, IGlimpseRequestContext glimpseRequestContext)
        {
            if (glimpseRequestContext.CurrentRuntimePolicy == RuntimePolicy.Off)
            {
                return null;
            }

            var requestStore = glimpseRequestContext.RequestStore;
            IDictionary<string, IDataStore> tabStorage;
            if (!requestStore.Contains(Constants.TabStorageKey))
            {
                tabStorage = new Dictionary<string, IDataStore>();
                requestStore.Set(Constants.TabStorageKey, tabStorage);
            }
            else
            {
                tabStorage = requestStore.Get<IDictionary<string, IDataStore>>(Constants.TabStorageKey);
            }

            IDataStore tabStore;
            if (!tabStorage.ContainsKey(tabName))
            {
                tabStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
                tabStorage.Add(tabName, tabStore);
            }
            else
            {
                tabStore = tabStorage[tabName];
            }

            return tabStore;
        }
    }
}