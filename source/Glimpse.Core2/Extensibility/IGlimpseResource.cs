using System.Collections.Generic;

namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpseResource
    {
        string Name { get; }
        ResourceResult Execute(IDictionary<string, string> parameters);
    }
}
