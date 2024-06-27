using System;
using System.Collections.Generic;
using Ecommerce.Application.Filters;

namespace Ecommerce.Application.Features.Products.Queries.Fe.FeGetPagingProducts
{
    public class FeGetPagingProductsParameter : RequestParameter
    {
        public List<int> id { get; set; }
    }
}
