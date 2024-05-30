using Ecommerce.Application.Features.ProductAttributeMappings.Commands.CreateProductAttributeMapping;
using Ecommerce.Application.Features.ProductAttributeMappings.Commands.DeleteProductAttributeMapping;
using Ecommerce.Application.Features.ProductAttributeMappings.Commands.UpdateProductAttributeMapping;
using Ecommerce.Application.Features.ProductAttributeMappings.Queries.GetPagedProdAttrMap;
using Ecommerce.Application.Features.ProductAttributeMappings.Queries.GetProdAttrMapById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Server.Controllers;

[Authorize]
[Route("api/product-attribute-mappings")]
public class ProductAttributeMappingController : BaseApiController
{
    [Obsolete]
    public ProductAttributeMappingController(
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
    {
    }

    // GET: api/product-attribute-mappings
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetPagedProdAttrMapByProductIdParameter filter)
    {
        return await EnforcePermissionAndExecute("product-attribute-mappings", "list", async () =>
        {
            return Ok(await Mediator.Send(new GetPagedProdAttrMapByProductIdQuery()
            {
                _productid = filter._productid,
                _end = filter._end,
                _start = filter._start,
                _order = filter._order,
                _sort = filter._sort,
                _filter = filter._filter
            }));
        });
    }

    // GET api/product-attribute-mappings/5
    [HttpGet("show/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return await EnforcePermissionAndExecute("product-attribute-mappings", "show", async () =>
        {
            return Ok(await Mediator.Send(new GetProdAttrMapByIdQuery { Id = id }));
        });
    }

    // POST api/product-attribute-mappings
    [HttpPost]
    public async Task<IActionResult> Post(CreateProductAttributeMappingCommand command)
    {
        return await EnforcePermissionAndExecute("product-attribute-mappings", "create", async () =>
        {
            return Ok(await Mediator.Send(command));
        });
    }

    // PUT api/product-attribute-mappings/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UpdateProductAttributeMappingCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        return await EnforcePermissionAndExecute("product-attribute-mappings", "edit", async () =>
        {
            return Ok(await Mediator.Send(command));
        });
    }

    // DELETE api/product-attribute-mappings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await EnforcePermissionAndExecute("product-attribute-mappings", "delete", async () =>
        {
            return Ok(await Mediator.Send(new DeleteProductAttributeMappingCommand { Id = id }));
        });
    }
}
