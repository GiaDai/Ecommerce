using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ecommerce.Application.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Features.Role.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest<Response<IdentityRole>>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Response<IdentityRole>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public UpdateRoleCommandHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<Response<IdentityRole>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByIdAsync(request.Id);
                if (role == null) throw new Exception($"Role Not Found.");
                role.Name = request.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return new Response<IdentityRole>(role);
                }
                throw new Exception(string.Join(", ", result.Errors));
            }
        }
    }
}
