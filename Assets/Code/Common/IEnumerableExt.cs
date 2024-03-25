using System;
using System.Collections.Generic;
using System.Linq;

namespace DGJ24
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExt
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        public static IEnumerable<T> FilterNull<T>(this IEnumerable<T?> items)
            where T : class
        {
            foreach (var item in items)
            {
                if (item != null)
                    yield return item;
            }
        }
        
             public static IEnumerable<T> FilterNull<T>(this IEnumerable<T?> items)
                    where T : struct
                {
                    foreach (var item in items)
                    {
                        if (item != null)
                            yield return item.Value;
                    }
                }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> items, T item)
        {
            return items.Except(Enumerable.Repeat(item, 1));
        }
    }
}
