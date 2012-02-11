using System;

namespace Glimpse.Core2.Extensibility
{
    [Flags]
    public enum RuntimeEvent
    {
        Initialize = 1,
        BeginRequest = 2,
        BeginSessionAccess = 4,
        ExecuteResource = 8,
        EndSessionAccess = 16,
        EndRequest = 32
    }
}