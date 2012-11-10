using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Mvc.Message
{
    public interface IFilterMessage : IExecutionMessage
    {
        FilterCategory Category { get; }
    }
}
