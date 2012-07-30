using System.Collections.Generic;

namespace Glimpse.Core2.Extensibility
{
    public interface IResource
    {
        string Name { get; }
        IEnumerable<ResourceParameterMetadata> Parameters { get; }
        IResourceResult Execute(IResourceContext context);
    }
}
