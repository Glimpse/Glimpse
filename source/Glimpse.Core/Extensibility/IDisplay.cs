using System;

namespace Glimpse.Core.Extensibility
{
    [Obsolete]
    public interface IDisplay
    {
        [Obsolete]
        string Name { get; }

        [Obsolete]
        object GetData(ITabContext context);
    }
}