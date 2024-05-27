using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ecommerce.Application.Wrappers;
using Ecommerce.Infrastructure.Identity.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Casbin;
using System.IO;

namespace Ecommerce.Infrastructure.Identity.Features.RoleClaim.Commands.CreateRoleClaim
{
    public class CreateRoleClaimCommand : IRequest<Response<IdentityRoleClaim<string>>>
    {
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string[] ClaimValue { get; set; }

        public class CreateRoleClaimCommandHandler : IRequestHandler<CreateRoleClaimCommand, Response<IdentityRoleClaim<string>>>
        {

            private readonly Enforcer _enforcer;
            private readonly string _webRootPath;
            private readonly IHostingEnvironment _env;
            private readonly IdentityContext _context;

            [System.Obsolete]
            public CreateRoleClaimCommandHandler(
                IHostingEnvironment env,
                IdentityContext context)
            {
                _env = env;
                _webRootPath = env.WebRootPath;
                _context = context;
                _enforcer = new Enforcer(Path.Combine(_webRootPath, "model.conf"), Path.Combine(_webRootPath, _env.IsProduction() ? "policy.csv" : "policy-dev.csv"));
            }

            public async Task<Response<IdentityRoleClaim<string>>> Handle(CreateRoleClaimCommand request, CancellationToken cancellationToken)
            {
                var roleClaim = new IdentityRoleClaim<string>
                {
                    RoleId = request.RoleId,
                    ClaimType = request.ClaimType,
                    ClaimValue = string.Join("#", request.ClaimValue)
                };
                var role = await _context.Roles.FindAsync(request.RoleId);
                await _enforcer.RemoveFilteredPolicyAsync(0, role.Name, request.ClaimType);
                foreach (var value in request.ClaimValue)
                {
                    _enforcer.AddPolicy(role.Name, request.ClaimType, value);
                }
                await _enforcer.SavePolicyAsync();
                _context.RoleClaims.Add(roleClaim);
                await _context.SaveChangesAsync();

                return new Response<IdentityRoleClaim<string>>(roleClaim);
            }
        }
    }
}
