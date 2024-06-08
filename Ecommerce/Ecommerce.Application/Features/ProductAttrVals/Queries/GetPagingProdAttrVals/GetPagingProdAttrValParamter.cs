using System;
using System.Collections.Generic;
using Ecommerce.Application.Filters;

namespace Ecommerce.Application.Features.ProductAttrVals.Queries.GetPagingProductAttrVals
{
    public class GetPagingProdAttrValParamter : RequestParameter
    {
        public List<int> id { get; set; }
    }
}
