using System.Diagnostics;

namespace Glimpse.Core.Extensions
{
    public static class LongExtensions
    {
        public static double ConvertNanosecondsToMilliseconds(this long ticks)
        {
            var ns = 1000000000.0 * ticks / Stopwatch.Frequency;
            return ns / 1000000.0;
        }
    }
}
