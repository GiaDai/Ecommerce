using Ecommerce.Application.Filters;

namespace Ecommerce.Application.Features.ProductAttributeMappings.Queries.GetPagedProdAttrMap
{
    public class GetPagedProdAttrMapByProductIdParameter : RequestParameter
    {
        public int _productid { get; set; }
    }
}
