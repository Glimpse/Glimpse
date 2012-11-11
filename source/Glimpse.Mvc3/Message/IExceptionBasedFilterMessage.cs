using System;

namespace Glimpse.Mvc.Message
{
    public interface IExceptionBasedFilterMessage
    {
        Type ExceptionType { get; }
    }
}