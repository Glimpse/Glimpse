using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Glimpse.Core.Extensions
{
    public static class LongExtensions
    {
        public static double ConvertNanosecondsToMilliseconds(this long ticks)
        {
            var ns = 1000000000.0 * (double)ticks / Stopwatch.Frequency;
            return ns / 1000000.0;
        }
    }
}
