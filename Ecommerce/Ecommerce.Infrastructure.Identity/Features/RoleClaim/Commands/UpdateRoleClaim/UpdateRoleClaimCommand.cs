using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ecommerce.Application.Wrappers;
using Ecommerce.Infrastructure.Identity.Contexts;
using Microsoft.AspNetCore.Identity;
using Casbin;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Ecommerce.Infrastructure.Identity.Features.RoleClaim.Commands.UpdateRoleClaim
{
    public class UpdateRoleClaimCommand : IRequest<Response<IdentityRoleClaim<string>>>
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string[] ClaimValue { get; set; }

        public class UpdateRoleClaimCommandHandler : IRequestHandler<UpdateRoleClaimCommand, Response<IdentityRoleClaim<string>>>
        {
            private readonly Enforcer _enforcer;
            private readonly string _webRootPath;
            private readonly IdentityContext _context;

            [Obsolete]
            public UpdateRoleClaimCommandHandler(
                IdentityContext context,
                IHostingEnvironment hostingEnvironment
                )
            {
                _webRootPath = hostingEnvironment.WebRootPath;
                _context = context;
                _enforcer = new Enforcer(Path.Combine(_webRootPath, "model.conf"), Path.Combine(_webRootPath, "policy.csv"));
            }

            public async Task<Response<IdentityRoleClaim<string>>> Handle(UpdateRoleClaimCommand request, CancellationToken cancellationToken)
            {
                var roleClaim = await _context.RoleClaims.FindAsync(request.Id);
                if (roleClaim == null) throw new Exception($"RoleClaim Not Found.");
                var role = await _context.Roles.FindAsync(roleClaim.RoleId);
                await _enforcer.RemoveFilteredPolicyAsync(0, role.Name, request.ClaimType);
                foreach (var value in request.ClaimValue)
                {
                    await _enforcer.AddPolicyAsync(role.Name, request.ClaimType, value);
                }
                await _enforcer.SavePolicyAsync();

                roleClaim.ClaimType = request.ClaimType;
                roleClaim.ClaimValue = string.Join("#", request.ClaimValue);
                await _context.SaveChangesAsync();

                return new Response<IdentityRoleClaim<string>>(roleClaim);
            }
        }
    }
}
