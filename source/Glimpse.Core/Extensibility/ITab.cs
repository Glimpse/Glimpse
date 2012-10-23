using System;

namespace Glimpse.Core.Extensibility
{
    public interface ITab
    {
        string Name { get; }
        
        RuntimeEvent ExecuteOn { get; }
        
        Type RequestContextType { get; }
        
        object GetData(ITabContext context);
    }
}
