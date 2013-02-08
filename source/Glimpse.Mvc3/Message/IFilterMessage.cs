using System;

namespace Glimpse.Mvc.Message
{
    public interface IFilterMessage : IActionMessage 
    {
        FilterCategory Category { get; set; }

        Type ResultType { get; set; }
    }

    public static class FilterMessageExtension
    {
        public static T AsFilterMessage<T>(this T message, FilterCategory category, Type resultType)
            where T : IFilterMessage
        {
            message.Category = category;
            message.ResultType = resultType;

            return message;
        } 
    }
}
