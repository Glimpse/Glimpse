using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> ToEmptyIfNull<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return Enumerable.Empty<TSource>();
            }

            return source;
        }

        public static TSource[] ToArrayOrDefault<TSource>(this IEnumerable<TSource> source) 
        {
            if (source == null)
            {
                return null;
            }

            return source.ToArray();
        }

        public static List<TSource> ToListOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return null;
            }

            return source.ToList();
        }
    }
}
