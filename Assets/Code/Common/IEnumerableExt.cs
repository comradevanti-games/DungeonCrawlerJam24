using System;
using System.Collections.Generic;

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
        {
            foreach (var item in items)
            {
                if (item != null)
                    yield return item;
            }
        }
    }
}
