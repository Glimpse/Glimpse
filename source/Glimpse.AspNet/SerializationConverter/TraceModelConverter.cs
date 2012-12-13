using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.AspNet.SerializationConverter
{
    public class TraceModelConverter : SerializationConverter<IList<TraceModel>>
    {
        public override object Convert(IList<TraceModel> obj)
        {
            var root = new TabSection("Category", "Trace", "From First", "From Last");
            foreach (var item in obj)
            {
                root.AddRow().Column(item.Category).Column(GenerateTabs(item)).Column(Math.Round(item.FromFirst, 2)).Column(Math.Round(item.FromLast, 2)).Style(item.Category);
            }

            return root.Build();
        } 

        private string GenerateTabs(TraceModel item)
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
