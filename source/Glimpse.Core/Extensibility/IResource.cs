using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    public interface IResource
    {
        string Name { get; }
        IEnumerable<ResourceParameterMetadata> Parameters { get; }
        IResourceResult Execute(IResourceContext context);
    }
}
