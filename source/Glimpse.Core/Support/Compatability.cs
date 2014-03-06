using System;

namespace Glimpse.Core.Support
{
    public static class Compatability
    {
        public static bool TryParseGuid(string input, out Guid output)
        {
#if NET35
            return global::Glimpse.Core.Backport.Net35Backport.TryParseGuid(input, out output);
#else
            return Guid.TryParse(input, out output);
#endif
        }
    }
}
