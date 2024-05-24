using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ecommerce.Application.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Features.Role.Commands.DeleteRoleById
{
    public class DeleteRoleByIdCommand : IRequest<Response<IdentityRole>>
    {
        public string Id { get; set; }
        public class DeleteRoleByIdCommandHandler : IRequestHandler<DeleteRoleByIdCommand, Response<IdentityRole>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            public DeleteRoleByIdCommandHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }
            public async Task<Response<IdentityRole>> Handle(DeleteRoleByIdCommand command, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByIdAsync(command.Id);
                if (role == null) throw new Exception($"Role Not Found.");
                await _roleManager.DeleteAsync(role);
                return new Response<IdentityRole>(role);
            }
        }
    }
}
