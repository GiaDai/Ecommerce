using System;
using System.Collections.Generic;
using Ecommerce.Application.Filters;

namespace Ecommerce.Application.Features.ProductAttrs.Queries.GetPagingProductAttrs
{
    public class GetPagingProductAttrParameter : RequestParameter
    {
        public List<int> id { get; set; }
    }
}
