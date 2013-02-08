using System.Linq;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Mvc.Message;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.Tab
{
    public class Execution : AspNetTab, IDocumentation, ITabSetup, ITabLayout, IKey
    {
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).AsKey().WidthInPixels(60);
                    r.Cell(1);
                    r.Cell(2);
                    r.Cell(3).WidthInPixels(60);
                    r.Cell(4).WidthInPixels(160);
                    r.Cell(5).WidthInPixels(160);
                    r.Cell("{{6}}.{{7}}"); 
                    r.Cell(8).WidthInPercent(15).Suffix(" ms").Class("mono").AlignRight();
                }).Build();

        public override string Name
        {
            get { return "Execution"; }
        }

        public string Key
        {
            get { return "glimpse_execution"; }
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
            var actionFilterMessages = context.GetMessages<IExecutionMessage>();

            return actionFilterMessages.Select(message => new ExecutionModel(message)).ToList();
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<IExecutionMessage>();
        }
    }
}