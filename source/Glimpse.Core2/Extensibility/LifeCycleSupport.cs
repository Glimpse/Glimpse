using System;

namespace Glimpse.Core2.Extensibility
{
    [Flags]
    public enum LifeCycleSupport
    {
        EndRequest = 1,
        SessionAccessEnd = 2,
        SessionAccessBegin = 4,
        BeginRequest = 8
    }
}
