using System.Collections.Generic;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public interface IResourceContext:IContext
    {
        IDictionary<string, string> Parameters { get; }
        IReadOnlyPersistanceStore PersistanceStore { get; }
    }
}