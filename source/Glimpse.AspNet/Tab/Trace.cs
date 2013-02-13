using System;
using Glimpse.AspNet.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.AspNet.Tab
{
    public class Trace : ITab, ITabSetup, IDocumentation, ITabLayout, IKey
    {
        public const string TabKey = "glimpse_trace";

        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).AsKey().WidthInPixels(100);
                    r.Cell(1);
                    r.Cell(2).WidthInPercent(15).Suffix(" ms").AlignRight().Prefix("T+ ").Class("mono");
                    r.Cell(3).WidthInPercent(15).Suffix(" ms").AlignRight().Class("mono"); 
                }).Build();

        public string Name
        {
            get { return "Trace"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Plugin/Trace"; }
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        public Type RequestContextType
        {
            get { return null; }
        }

        public string Key
        {
            get { return TabKey; }
        }

        public object GetLayout()
        {
            return Layout;
        }

        public object GetData(ITabContext context)
        {
            var data = context.GetMessages<ITraceMessage>();
            return data;
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ITraceMessage>();
        }
    }
}
