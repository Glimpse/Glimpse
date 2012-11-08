using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Message;
using Glimpse.Mvc.Model;

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

            return actionFilterMessages.Select(message => new ExecutionModel(message)).ToList();
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