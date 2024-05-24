using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Features.Role.Queries.GetRoleById
{
    public class GetRoleByIdQuery : IRequest<Response<IdentityRole>>
    {
        public string Id { get; set; }
        public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Response<IdentityRole>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public GetRoleByIdQueryHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<Response<IdentityRole>> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByIdAsync(query.Id.ToString());
                if (role == null) throw new ApiException($"Role Not Found.");

                return new Response<IdentityRole>(role);
            }
        }
    }
}
