using System;

namespace Glimpse.Mvc.Message
{
    public interface IFilterMessage : IActionBaseMessage
    {
        FilterCategory Category { get; }

        Type ResultType { get; }
    }
}
