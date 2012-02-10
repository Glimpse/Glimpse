using System;

namespace Glimpse.Core2
{
    [Flags]
    public enum RuntimePolicy
    {
        Off = 1,
        Ignore = 2,
        PersistResults = 4,
        ModifyResponseHeaders = 8 | PersistResults,
        ModifyResponseBody = 16 | ModifyResponseHeaders,
        DisplayGlimpseClient = 32 | ModifyResponseBody,
        On = DisplayGlimpseClient
    }
}