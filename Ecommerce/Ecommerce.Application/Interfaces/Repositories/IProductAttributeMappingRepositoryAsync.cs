using System.Threading.Tasks;
using Ecommerce.Application.Features.ProductAttrMaps.Queries.GetPagedProdAttrMap;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IProductAttributeMappingRepositoryAsync : IGenericRepositoryAsync<ProductAttributeMapping>
    {
        Task<PagedList<ProductAttributeMapping>> GetPagedProdAttrMapAsync(GetPagingProdAttrMapParameter parameter);
    }
}
