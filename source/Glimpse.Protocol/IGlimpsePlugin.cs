using System.Collections.Generic;
using System.Web;

namespace Glimpse.Protocol
{
    public interface IGlimpsePlugin
    {
        string Name { get; }
        IDictionary<string, string> GetData(HttpApplication application);
    }
}
