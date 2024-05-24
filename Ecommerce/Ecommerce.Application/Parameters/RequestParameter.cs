using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Application.Filters
{
    public class RequestParameter
    {
        public int _start { get; set; }
        public int _end { get; set; }
        public string _sort { get; set; }
        public string _order { get; set; }
        public List<string> _filter { get; set; }
        public RequestParameter()
        {
            this._start = 0;
            this._end = 10;
            this._sort = "Id";
            this._order = "asc";
            this._filter = new List<string>();
        }
        public RequestParameter(int start, int end)
        {
            this._start = start < 1 ? 0 : start;
            this._end = end > 10 ? 10 : end;
        }
    }
}
