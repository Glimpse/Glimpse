using System;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility; 

namespace Glimpse.AspNet.SerializationConverter
{ 
    public class TimelineEventModelConverter : SerializationConverter<TimelineEventModel>
    {
        public override object Convert(TimelineEventModel obj)
        {
            return new { obj.Title, obj.Category, obj.SubText, obj.StartTime, obj.Details, obj.Duration, obj.StartPoint };
        } 
    }
}
