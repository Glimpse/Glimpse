using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.Tab
{
    public class Execution : AspNetTab, ITabSetup
    {
        private const string TabStoreKey = "IActionFilterMessageKey";

        public override string Name
        {
            get { return "Execution"; }
        }

        public override object GetData(ITabContext context)
        {
            var actionFilterMessages = context.TabStore.Get<IList<IExecutionMessage>>(TabStoreKey);
            return actionFilterMessages;
        }

        public void Setup(ITabSetupContext context)
        {
            context.MessageBroker.Subscribe<IExecutionMessage>(message => PersistActionFilterMessage(message, context));
        }

        private static void PersistActionFilterMessage(IExecutionMessage message, ITabSetupContext context)
        {
            var tabStore = context.GetTabStore();

            if (!tabStore.Contains(TabStoreKey))
            {
                tabStore.Set(TabStoreKey, new List<IExecutionMessage>());
            }

            var messages = tabStore.Get<IList<IExecutionMessage>>(TabStoreKey);

            messages.Add(message);
        }
    }
}