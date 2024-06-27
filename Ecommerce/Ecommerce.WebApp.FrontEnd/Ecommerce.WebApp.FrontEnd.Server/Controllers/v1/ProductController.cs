
using Ecommerce.Application.Features.Products.Queries.Fe.FeGetPagingProducts;
using Ecommerce.Application.Features.Products.Queries.GetAllProducts;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.FrontEnd.Server.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductController : BaseApiController
{
    [Obsolete]
    public ProductController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
    {
    }

    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FeGetPagingProductsParameter filter)
    {
        return Ok(await Mediator.Send(new FeGetPagingProductsQuery()
        {
            id = filter.id,
            _end = filter._end,
            _start = filter._start,
            _order = filter._order,
            _sort = filter._sort,
            _filter = filter._filter
        }));
    }
}
