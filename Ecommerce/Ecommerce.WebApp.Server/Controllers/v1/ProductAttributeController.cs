
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Features.ProductAttributes.Queries.GetAllProductAttributes;
using Ecommerce.Application.Features.ProductAttributes.Queries.GetProductAttributeById;
using Ecommerce.Application.Features.ProductAttributes.Commands.CreateProductAttribute;
using Ecommerce.Application.Features.ProductAttributes.Commands.UpdateProductAttribute;
using Ecommerce.Application.Features.ProductAttributes.Commands.DeleteProductAttributeById;

namespace Ecommerce.WebApp.Server.Controllers;

[Authorize]
[Route("api/product-attributes")]
public class ProductAttributeController : BaseApiController
{
    [Obsolete]
    public ProductAttributeController(
        Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
    {
    }

    // GET: api/product-attributes
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductAttributeParameter filter)
    {
        return await EnforcePermissionAndExecute("product-attributes", "list", async () =>
        {
            return Ok(await Mediator.Send(new GetAllProductAttributesQuery()
            {
                _end = filter._end,
                _start = filter._start,
                _order = filter._order,
                _sort = filter._sort,
                _filter = filter._filter
            }));
        });
    }

    // GET api/product-attributes/5
    [HttpGet("show/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return await EnforcePermissionAndExecute("product-attributes", "show", async () =>
        {
            return Ok(await Mediator.Send(new GetProductAttributeByIdQuery { Id = id }));
        });
    }

    // POST api/product-attributes
    [HttpPost]
    public async Task<IActionResult> Post(CreateProductAttributeCommand command)
    {
        return await EnforcePermissionAndExecute("product-attributes", "create", async () =>
        {
            return Ok(await Mediator.Send(command));
        });
    }

    // PUT api/product-attributes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UpdateProductAttributeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        return await EnforcePermissionAndExecute("product-attributes", "edit", async () =>
        {
            return Ok(await Mediator.Send(command));
        });
    }

    // DELETE api/product-attributes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await EnforcePermissionAndExecute("product-attributes", "delete", async () =>
        {
            return Ok(await Mediator.Send(new DeleteProductAttributeByIdCommand { Id = id }));
        });
    }
}
