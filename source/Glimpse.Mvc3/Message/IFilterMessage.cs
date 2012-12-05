using System; 
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public interface IFilterMessage : IActionBaseMessage, ITimelineMessage
    {
        FilterCategory Category { get; }

        Type ResultType { get; }
    }
}
