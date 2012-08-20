using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    public class TabResultConverter:SerializationConverter<TabResult>
    {
        public override object Convert(TabResult result)
        {
            return new Dictionary<string, object>
                       {
                           {"data", result.Data},
                           {"name", result.Name},
                       };
        }
    }
}