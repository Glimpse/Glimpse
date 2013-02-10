using System.Collections.Generic;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.AspNet.SerializationConverter
{
    public class SessionModelConverter : SerializationConverter<List<SessionModel>>
    {
        public override object Convert(List<SessionModel> obj)
        {
            var root = new TabSection("Key", "Value", "Type");
            foreach (var item in obj)
            {
                var row = root.AddRow().Column(item.Key);

                if (item.Type != null && !item.Type.IsSerializable)
                {
                    row.Column("Non serializable type :(").Emphasis();
                }
                else
                {
                    row.Column(item.Value);
                }

                row.Column(item.Type);
            }

            return root.Build();
        }
    }
}
