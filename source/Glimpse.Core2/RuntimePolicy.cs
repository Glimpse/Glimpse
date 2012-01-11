using System;

namespace Glimpse.Core2
{
    [Flags]
    public enum RuntimePolicy
    {
        Off = 1,
        PersistResults = 2,
        ModifyResponseHeaders = 4 | PersistResults,
        ModifyResponseBody = 8 | ModifyResponseHeaders,
        DisplayGlimpseClient = 16 | ModifyResponseBody,
        On = DisplayGlimpseClient
    }
}