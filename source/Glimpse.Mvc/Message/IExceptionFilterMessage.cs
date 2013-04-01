using System;

namespace Glimpse.Mvc.Message
{
    public interface IExceptionFilterMessage : IFilterMessage
    {
        Type ExceptionType { get; set; }

        bool ExceptionHandled { get; set; }
    }

    public static class ExceptionBasedFilterMessageExtension
    {
        public static T AsExceptionFilterMessage<T>(this T message, Type exceptionType, bool exceptionHandled)
            where T : IExceptionFilterMessage
        {
            message.ExceptionType = exceptionType;
            message.ExceptionHandled = exceptionHandled;

            return message;
        }
    }
}