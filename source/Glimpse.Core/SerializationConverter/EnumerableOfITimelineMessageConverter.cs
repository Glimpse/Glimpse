using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Core.SerializationConverter
{
    public class EnumerableOfITimelineMessageConverter : SerializationConverter<IEnumerable<ITimelineMessage>>
    {
        public override object Convert(IEnumerable<ITimelineMessage> obj)
        {
            return obj.Select(message => new
                {
                    title = message.EventName,
                    startTime = message.StartTime,
                    duration = message.Duration,
                    startPoint = message.Offset,
                    category = message.EventCategory.Name
                });
        }
    }
}