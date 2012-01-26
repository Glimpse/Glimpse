using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core2.Extensions
{
    public static class ListExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> range)
        {
            foreach (var item in range.Where(item => !list.Contains(item)))
            {
                list.Add(item);
            }
        }
    }
}
