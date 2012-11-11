using System;

namespace Glimpse.Mvc.Message
{
    public interface IFilterMessage : IExecutionMessage
    {
        FilterCategory Category { get; }

        Type ResultType { get; }
    }
}
