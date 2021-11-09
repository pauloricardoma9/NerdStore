using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> itens, Action<T> itemAction)
        {
            foreach (var item in itens)
            {
                itemAction(item);
            }
        }
    }
}
