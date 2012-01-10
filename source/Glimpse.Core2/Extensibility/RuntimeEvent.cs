using System;

namespace Glimpse.Core2.Extensibility
{
    [Flags]
    public enum RuntimeEvent
    {
        Initialize = 1,
        BeginRequest = 2,
        ExecuteTabs = 4,
        ExecuteResource = 8,
        EndRequest = 16
    }
}