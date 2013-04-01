using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Mvc.Message
{
    public interface IBoundedFilterMessage : IFilterMessage
    {
        FilterBounds Bounds { get; set; }
    }

    public static class BoundedFilterMessageExtension
    {
        public static T AsBoundedFilterMessage<T>(this T message, FilterBounds bounds)
            where T : IBoundedFilterMessage
        {
            message.Bounds = bounds; 

            return message;
        }
    }
}
