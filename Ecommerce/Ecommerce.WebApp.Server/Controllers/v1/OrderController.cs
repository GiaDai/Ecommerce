using Ecommerce.Application.Features.Orders.Commands.PlaceOrder;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Server.Controllers.v1;

// [Authorize]
[Route("api/orders")]
public class OrderController : BaseApiController
{
    [Obsolete]
    public OrderController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
    {
    }

    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> Post(PlaceOrderCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
