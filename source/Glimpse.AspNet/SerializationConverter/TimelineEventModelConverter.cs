using System;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility; 

namespace Glimpse.AspNet.SerializationConverter
{ 
    public class TimelineEventModelConverter : SerializationConverter<TimelineEventModel>
    {
        public override object Convert(TimelineEventModel obj)
        {
            return new { obj.Title, obj.Category, obj.SubText, obj.StartTime, obj.Details, Duration = Math.Round(obj.Duration, 2), StartPoint = Math.Round(obj.StartPoint, 2) };
        } 
    }
}
