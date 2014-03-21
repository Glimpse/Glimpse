
using Glimpse.Core.Message;

namespace Glimpse.WebApi.Core.Message
{
    public interface IExecutionMessage : ISourceMessage, IActionMessage, ITimelineMessage
    {
    }
}
