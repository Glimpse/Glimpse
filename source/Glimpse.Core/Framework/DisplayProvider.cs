using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    internal class DisplayProvider : BaseDataProvider
    {
        public DisplayProvider(IConfiguration configuration, ActiveGlimpseRequestContexts activeGlimpseRequestContexts)
            : base(configuration, activeGlimpseRequestContexts)
        {
        }

        public void Setup()
        {
            var logger = Configuration.Logger;
            var messageBroker = Configuration.MessageBroker;

            // TODO: Fix this to IDisplay no longer uses I*Tab*Setup
            var displaysThatRequireSetup = Configuration.Displays.Where(display => display is ITabSetup).Select(display => display);
            foreach (ITabSetup display in displaysThatRequireSetup)
            {
                var key = KeyCreator.Create(display);
                try
                {
                    var setupContext = new TabSetupContext(logger, messageBroker, () => GetTabStore(key, CurrentRequestContext));
                    display.Setup(setupContext);
                }
                catch (Exception exception)
                {
                    logger.Error(Resources.InitializeTabError, exception, key);
                }
            }
        }

        public void Execute(IGlimpseRequestContext glimpseRequestContext)
        {
            var runtimeContext = glimpseRequestContext.RequestResponseAdapter.RuntimeContext;
            var messageBroker = Configuration.MessageBroker;

            var displayResultsStore = GetResultsStore(glimpseRequestContext);
            var logger = Configuration.Logger;

            foreach (var display in Configuration.Displays)
            {
                TabResult result; // TODO: Rename now that it is no longer *just* tab results
                var key = KeyCreator.Create(display);
                try
                {
                    var displayContext = new TabContext(runtimeContext, GetTabStore(key, glimpseRequestContext), logger, messageBroker); // TODO: Do we need a DisplayContext?
                    var displayData = display.GetData(displayContext);

                    result = new TabResult(display.Name, displayData);
                }
                catch (Exception exception)
                {
                    result = new TabResult(display.Name, exception.ToString());
                    logger.Error(Resources.ExecuteTabError, exception, key);
                }

                if (displayResultsStore.ContainsKey(key))
                {
                    displayResultsStore[key] = result;
                }
                else
                {
                    displayResultsStore.Add(key, result);
                }
            }
        }

        public IDictionary<string, TabResult> GetResultsStore(IGlimpseRequestContext glimpseRequestContext)
        {
            return GetResultsStore<Dictionary<string, TabResult>>(glimpseRequestContext, Constants.DisplayResultsDataStoreKey);
        }
    }
}