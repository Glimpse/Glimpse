using System.Collections.Generic;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    public interface IResourceContext:IContext
    {
        IDictionary<string, string> Parameters { get; }
        IReadOnlyPersistenceStore PersistenceStore { get; }
    }
}