using Ecommerce.Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsParameter : RequestParameter
    {
        public List<int> id { get; set; }
    }
}
