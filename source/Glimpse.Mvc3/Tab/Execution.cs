using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Message;
using Glimpse.Mvc.Model;
using Glimpse.Core.Plugin.Assist;

namespace Glimpse.Mvc.Tab
{
    public class Execution : AspNetTab, IDocumentation, ITabSetup, ITabLayout
    {
        private const string TabStoreKey = "IActionFilterMessageKey";

        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).AsKey().WidthInPixels(60);
                    r.Cell(1).WidthInPixels(60);
                    r.Cell(2).WidthInPixels(160);
                    r.Cell(3).WidthInPixels(160);
                    r.Cell(4);
                    r.Cell(5);
                    r.Cell(6).WidthInPercent(15).Suffix(" ms");
                }).Build();

        public override string Name
        {
            get { return "Execution"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Plugin/Execution"; }
        }

        public object GetLayout()
        {
            return Layout;
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