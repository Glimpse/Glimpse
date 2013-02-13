using System.Collections.Generic;
using Glimpse.Core.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.Core.SerializationConverter
{
    public class ListOfTraceMessageConverter : SerializationConverter<IEnumerable<ITraceMessage>>
    {
        public override object Convert(IEnumerable<ITraceMessage> obj)
        {
            var root = new TabSection("Category", "Trace", "From Request Start", "From Last");
            foreach (var item in obj)
            {
                root.AddRow().Column(item.Category).Column(GenerateTabs(item)).Column(item.FromFirst).Column(item.FromLast).Style(item.Category);
            }

            return root.Build();
        } 

        private string GenerateTabs(ITraceMessage item)
        {
            var tabs = string.Empty;
            for (var i = 0; i < item.IndentLevel; i++)
            {
                tabs += "\t";
            }

            return tabs + item.Message;
        }
    }
}
