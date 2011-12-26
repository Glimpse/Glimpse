using System.Collections.Generic;

namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpseResourceEndpointConfiguration
    {
        string GenerateUrl(string resourceName, string version);
        string GenerateUrl(string resourceName, string version, IDictionary<string, string> parameters);
    }
}
