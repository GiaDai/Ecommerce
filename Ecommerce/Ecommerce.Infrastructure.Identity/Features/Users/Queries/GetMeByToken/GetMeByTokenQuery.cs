using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ecommerce.Application.Wrappers;

namespace Ecommerce.Infrastructure.Identity.Features.Users.Queries.GetMeByToken
{
    public class GetMeByTokenQuery : IRequest<Response<GetMeByTokenQueryModel>>
    {
        public ClaimsIdentity Identity { get; set; }

        public class GetMeByTokenQueryHandler : IRequestHandler<GetMeByTokenQuery, Response<GetMeByTokenQueryModel>>
        {

            public GetMeByTokenQueryHandler()
            {
            }

            public async Task<Response<GetMeByTokenQueryModel>> Handle(GetMeByTokenQuery request, CancellationToken cancellationToken)
            {
                var email = FindClaimValue(request.Identity, ClaimTypes.Email);
                var name = FindClaimValue(request.Identity, ClaimTypes.NameIdentifier);
                var fullname = FindClaimValue(request.Identity, "fullname");
                var roles = FindAllClaimValues(request.Identity, ClaimTypes.Role);
                var uid = FindClaimValue(request.Identity, "uid");
                var avatarUrl = FindClaimValue(request.Identity, "AvatarUrl");
                var avatarUid = FindClaimValue(request.Identity, "AvatarUid");
                await Task.CompletedTask;
                return new Response<GetMeByTokenQueryModel>(new GetMeByTokenQueryModel
                {
                    Email = email,
                    Name = name,
                    Fullname = fullname,
                    Roles = roles.ToArray(),
                    Uid = uid,
                    AvatarUrl = avatarUrl,
                    AvatarUid = avatarUid
                });

            }
        }

        private static string FindClaimValue(ClaimsIdentity identity, string claimType)
        {
            return identity.FindFirst(claimType)?.Value;
        }

        private static IEnumerable<string> FindAllClaimValues(ClaimsIdentity identity, string claimType)
        {
            return identity.FindAll(claimType).Select(x => x.Value);
        }
    }
}
