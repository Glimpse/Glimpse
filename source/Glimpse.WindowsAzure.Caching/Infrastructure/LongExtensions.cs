using System;

namespace Glimpse.WindowsAzure.Caching.Infrastructure
{
    public static class LongExtensions
    {
        public static string ToBytesHuman(this long bytes)
        {
            const int scale = 1024;
            string[] orders = new[] { "TB", "GB", "MB", "KB", "bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes >= max)
                {
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);
                }

                max /= scale;
            }

            return "0 bytes";
        }
    }
}