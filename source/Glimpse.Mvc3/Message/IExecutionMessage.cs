using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public interface IExecutionMessage : ISourceMessage, IChildActionMessage, ITimelineMessage
    {
    }
}
