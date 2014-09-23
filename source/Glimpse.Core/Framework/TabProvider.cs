using System;
using System.Collections.Generic;
using System.Linq; 
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;
#if NET35
using Glimpse.Core.Backport;
#endif

namespace Glimpse.Core.Framework
{
    internal class TabProvider : BaseDataProvider
    {
        public TabProvider(IReadOnlyConfiguration configuration, ActiveGlimpseRequestContexts activeGlimpseRequestContexts)
            : base(configuration, activeGlimpseRequestContexts)
        {
        }

        public void Setup()
        {
            var logger = Configuration.Logger;
            var messageBroker = Configuration.MessageBroker;

            var tabsThatRequireSetup = Configuration.Tabs.Where(tab => tab is ITabSetup).Select(tab => tab);
            foreach (ITabSetup tab in tabsThatRequireSetup)
            {
                var key = KeyCreator.Create(tab);
                try
                {
                    var setupContext = new TabSetupContext(logger, messageBroker, () => GetTabStore(key, CurrentRequestContext));
                    tab.Setup(setupContext);
                }
                catch (Exception exception)
                {
                    logger.Error(Resources.InitializeTabError, exception, key);
                }
            }
        }

        public void Execute(IGlimpseRequestContext glimpseRequestContext, RuntimeEvent runtimeEvent)
        {
            var logger = Configuration.Logger;
            var messageBroker = Configuration.MessageBroker;

            var runtimeContext = glimpseRequestContext.RequestResponseAdapter.RuntimeContext;
            var frameworkProviderRuntimeContextType = runtimeContext.GetType();

            var tabResultsStore = GetResultsStore(glimpseRequestContext);

            // Only use tabs that either don't specify a specific context type, or have a context 
            // type that matches the current framework provider's.
            var runtimeTabs = Configuration.Tabs.Where(tab => tab.RequestContextType == null || frameworkProviderRuntimeContextType.IsSubclassOf(tab.RequestContextType) || tab.RequestContextType == frameworkProviderRuntimeContextType);
            var supportedRuntimeTabs = runtimeTabs.Where(p => p.ExecuteOn.HasFlag(runtimeEvent)); 
            foreach (var tab in supportedRuntimeTabs)
            {
                TabResult result;
                var key = KeyCreator.Create(tab);
                try
                {
                    var tabContext = new TabContext(runtimeContext, GetTabStore(key, glimpseRequestContext), logger, messageBroker);
                    var tabData = tab.GetData(tabContext);

                    var tabSection = tabData as TabSection;
                    if (tabSection != null)
                    {
                        tabData = tabSection.Build();
                    }

                    result = new TabResult(tab.Name, tabData);
                }
                catch (Exception exception)
                {
                    result = new TabResult(tab.Name, exception.ToString());
                    logger.Error(Resources.ExecuteTabError, exception, key);
                }

                if (tabResultsStore.ContainsKey(key))
                {
                    tabResultsStore[key] = result;
                }
                else
                {
                    tabResultsStore.Add(key, result);
                }
            }
        }
         
        public IDictionary<string, TabResult> GetResultsStore(IGlimpseRequestContext glimpseRequestContext)
        {
            return GetResultsStore<Dictionary<string, TabResult>>(glimpseRequestContext, Constants.TabResultsDataStoreKey);
        } 
    }
}
