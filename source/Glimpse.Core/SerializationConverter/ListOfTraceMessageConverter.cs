using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// Converter for <see cref="ITraceMessage"/>
    /// </summary>
    public class ListOfTraceMessageConverter : SerializationConverter<IEnumerable<ITraceMessage>>
    {
        /// <summary>
        /// Converts the specified object.
        /// </summary>
        /// <param name="obj">The object to transform.</param>
        /// <returns>The new object representation.</returns>
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
