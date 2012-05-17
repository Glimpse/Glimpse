using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.SerializationConverter
{
    public class TabResultConverter:SerializationConverter<TabResult>
    {
        public override IDictionary<string, object> Convert(TabResult result)
        {
            return new Dictionary<string, object>
                       {
                           {"data", result.Data},
                           {"name", result.Name},
                       };
        }
    }
}