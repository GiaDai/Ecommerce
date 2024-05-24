using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Identity.Features.Users.Queries.CreateUser;
using Ecommerce.Infrastructure.Identity.Features.Users.Queries.GetPagingUser;
using Ecommerce.Infrastructure.Identity.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.WebApp.Server.Controllers.Identity
{

    [Authorize]
    [Route("api/users")]
    public class UsersController : BaseApiController
    {
        [Obsolete]
        public UsersController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
        {
        }
        // GET: api/users?_start=0&_end=10&_order=asc&_sort=Id
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPagingUserParameter filter)
        {
            return await EnforcePermissionAndExecute("users", "list", async () =>
            {
                return Ok(await Mediator.Send(new GetPagingUserQuery()
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

        // GET: api/users/show/5
        [HttpGet("show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            return await EnforcePermissionAndExecute("users", "show", async () =>
            {
                return Ok(await Mediator.Send((new GetUserByIdQuery { Id = id })));
            });
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            return await EnforcePermissionAndExecute("users", "create", async () =>
            {
                return Ok(await Mediator.Send(command));
            });
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateUserCommand command)
        {
            return await EnforcePermissionAndExecute("users", "edit", async () =>
            {
                if (id != command.Id)
                {
                    return BadRequest();
                }
                return Ok(await Mediator.Send(command));
            });
        }


        // DELETE: api/users/delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return await EnforcePermissionAndExecute("users", "delete", async () =>
            {
                return Ok(await Mediator.Send(new DeleteUserByIdCommand { Id = id }));
            });
        }
    }
}
