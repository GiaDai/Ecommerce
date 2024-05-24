using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ecommerce.Application.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Features.Role.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<Response<IdentityRole>>
    {
        public string Name { get; set; }

        public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response<IdentityRole>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public CreateRoleCommandHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<Response<IdentityRole>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
            {
                var role = new IdentityRole(request.Name);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return new Response<IdentityRole>(role);
                }
                throw new Exception(string.Join(", ", result.Errors));
            }
        }
    }
}
