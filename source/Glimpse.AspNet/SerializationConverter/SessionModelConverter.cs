using System.Collections.Generic;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.SerializationConverter
{
    public class SessionModelConverter : SerializationConverter<List<SessionModel>>
    {
        public override object Convert(List<SessionModel> obj)
        {
            var result = new List<object[]> { new[] { "Key", "Value", "Type" } };
            foreach (var item in obj)
            {
                result.Add(new[] { item.Key, item.Value, item.Type });
            }

            return result;
        }
    }
}
