using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glimpse.Core.Message;

namespace Glimpse.WebApi.Message
{
    public interface IExecutionMessage : ISourceMessage, IActionMessage, ITimelineMessage
    {
    }
}
