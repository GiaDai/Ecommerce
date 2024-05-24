using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.Wrappers
{
    public class PagedList<T> : List<T>
    {
        public int _start { get; private set; }
        public int _pages { get; private set; }
        public int _end { get; private set; }
        public int _total { get; private set; }

        public bool _hasPrevious => _start > 1;
        public bool _hasNext => _start < _pages;

        public PagedList(List<T> items, int count, int _start, int _end)
        {
            _total = count;
            this._end = _end;
            this._start = _start;
            _pages = (int)Math.Ceiling(count / (double)_end);
            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int _start, int _end)
        {
            var count = source.Count();
            var items = await source.Skip(_start).Take(_end - _start).ToListAsync();
            return new PagedList<T>(items, count, _start, _end);
        }
    }
}
