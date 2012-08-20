using System;

namespace Glimpse.Core
{
    [Flags]
    public enum RuntimePolicy
    {
        Off = 1,
        ExecuteResourceOnly = 2 | Off,
        PersistResults = 4,
        ModifyResponseHeaders = 8 | PersistResults,
        ModifyResponseBody = 16 | ModifyResponseHeaders,
        DisplayGlimpseClient = 32 | ModifyResponseBody,
        On = DisplayGlimpseClient
    }
}