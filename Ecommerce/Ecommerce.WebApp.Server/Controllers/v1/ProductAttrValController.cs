
using Ecommerce.Application;
using Ecommerce.Application.Features.ProductAttrVals.Commands.CreateProdAttrVal;
using Ecommerce.Application.Features.ProductAttrVals.Commands.DeleteProdAttrValById;
using Ecommerce.Application.Features.ProductAttrVals.Commands.UpdateProdAttrVal;
using Ecommerce.Application.Features.ProductAttrVals.Queries.GetPagingProductAttrVals;
using Ecommerce.Application.Features.ProductAttrVals.Queries.GetProdAttrValueById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Server.Controllers.v1;

[Authorize]
[Route("api/product-attribute-values")]
public class ProductAttrValController : BaseApiController
{
    [Obsolete]
    public ProductAttrValController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
    {
    }

    // GET: api/product-attribute-values
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetPagingProdAttrValParamter filter)
    {
        return await EnforcePermissionAndExecute("product-attribute-values", "list", async () =>
        {
            return Ok(await Mediator.Send(new GetPagingProdAttrValQuery()
            {
                id = filter.id,
                _end = filter._end,
                _start = filter._start,
                _order = filter._order,
                _sort = filter._sort,
                _filter = filter._filter
            }));
        });
    }

    // GET api/product-attribute-values/5
    [HttpGet("show/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return await EnforcePermissionAndExecute("product-attribute-values", "show", async () =>
        {
            return Ok(await Mediator.Send(new GetProdAttrValueByIdQuery { Id = id }));
        });
    }

    // POST api/product-attribute-values
    [HttpPost]
    public async Task<IActionResult> Post(CreateProdAttrValCommand command)
    {
        return await EnforcePermissionAndExecute("product-attribute-values", "create", async () =>
        {
            return Ok(await Mediator.Send(command));
        });
    }

    // PUT api/product-attribute-values/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UpdateProdAttrValCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        return await EnforcePermissionAndExecute("product-attribute-values", "edit", async () =>
        {
            return Ok(await Mediator.Send(command));
        });
    }

    // DELETE api/product-attribute-values/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await EnforcePermissionAndExecute("product-attribute-values", "delete", async () =>
        {
            return Ok(await Mediator.Send(new DeleteProdAttrValByIdCommand { Id = id }));
        });
    }
}
