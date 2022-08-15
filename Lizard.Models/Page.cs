using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class Page<T>
    {
        public Page(IEnumerable<T> items, int total, int limit, int offset)
        {
            Items = items;
            Total = total;
            Limit = limit;
            Offset = offset;
        }

        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }

    public static class PageExtensions
    {
        public static Page<T> ToPage<T>(this IEnumerable<T> items, int total, int limit, int offset)
        {
            return new Page<T>(items, total, limit, offset);
        }
    }
}
