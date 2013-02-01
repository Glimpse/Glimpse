using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Model;

namespace Glimpse.Core.SerializationConverter
{ 
    public class TimelineEventModelConverter : SerializationConverter<TimelineEventModel>
    {
        public override object Convert(TimelineEventModel obj)
        {
            return new { obj.Title, obj.Category, obj.SubText, obj.StartTime, obj.Details, obj.Duration, obj.StartPoint };
        } 
    }
}
