using System;

namespace Glimpse.Core2
{
    [Flags]
    public enum RuntimePolicy
    {
        Off = 1,
        ModifyResponseHeaders = 2,
        ModifyResponseBody = 4,
        DisplayGlimpseClient = 8,
        On = ModifyResponseBody | ModifyResponseHeaders | DisplayGlimpseClient
    }
}