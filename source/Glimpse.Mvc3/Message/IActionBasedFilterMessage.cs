using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Mvc.Message
{
    public interface IActionBasedMessage
    {
        string ControllerName { get;  }

        string ActionName { get; }
    }
}
