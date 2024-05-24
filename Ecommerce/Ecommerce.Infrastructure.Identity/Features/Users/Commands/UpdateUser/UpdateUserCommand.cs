using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Wrappers;
using Ecommerce.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity
{
    public class UpdateUserCommand : IRequest<Response<ApplicationUser>>
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public bool EmailConfirmed { get; set; }

        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<ApplicationUser>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly UserManager<ApplicationUser> _userManager;
            public UpdateUserCommandHandler(
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager
                )
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<Response<ApplicationUser>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(command.Id);
                if (user == null) throw new ApiException($"User Not Found.");
                user.EmailConfirmed = command.EmailConfirmed;
                await _userManager.UpdateAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                // Add user claim for avatar

                if (roles.Count > 0)
                {
                    var role = await _roleManager.FindByIdAsync(command.RoleId);
                    if (!roles.Contains(command.RoleId))
                    {
                        await _userManager.RemoveFromRolesAsync(user, roles);
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
                return new Response<ApplicationUser>(user);
            }
        }
    }
}
