using AutoMapper;
using MediatR;
using Ecommerce.Application.Wrappers;
using Ecommerce.Infrastructure.Identity.Contexts;
using Ecommerce.Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Ecommerce.Infrastructure.Identity.Features.Users.Queries.GetPagingUser
{
    public class GetPagingUserQuery : IRequest<Response<object>>
    {
        public List<string> id { get; set; }
        public int _start { get; set; }
        public int _end { get; set; }
        public string _sort { get; set; }
        public string _order { get; set; }
        public List<string> _filter { get; set; }

        public class GetPagingUserHanlder : IRequestHandler<GetPagingUserQuery, Response<object>>
        {
            private readonly IMapper _mapper;
            private readonly IdentityContext _context;
            public GetPagingUserHanlder(IdentityContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Response<object>> Handle(GetPagingUserQuery request, CancellationToken cancellationToken)
            {
                if (request.id != null && request.id.Count > 0)
                {
                    var userIds = await _context.Users
                        .AsNoTracking()
                        .Where(x => request.id.Contains(x.Id))
                        .ToListAsync();
                    return new Response<object>(true, userIds, message: "Success");
                }
                if (request.id != null && request.id.Count == 0)
                {
                    var userIds = await _context.Users
                        .AsNoTracking()
                        .ToListAsync();
                    return new Response<object>(true, userIds, message: "Success");
                }
                var userQuery = _context.Users
                    .Join(_context.UserRoles,
                        user => user.Id,
                        userRole => userRole.UserId,
                        (user, userRole) => new { user, userRole })
                    // .GroupJoin(_context.UserClaims,
                    //     userRole => userRole.user.Id,
                    //     userClaim => userClaim.UserId,
                    //     (userRole, userClaims) => new { userRole, userClaims })
                    // .SelectMany(
                    //     uc => uc.userClaims.DefaultIfEmpty(),
                    //     (uc, userClaim) => new { uc.userRole.user, uc.userRole.userRole, userClaim })
                    // .Where(uc => uc.userClaim == null || uc.userClaim.ClaimType == "AvatarUrl")
                    .Select(uc => new GetPagingUserViewModel
                    {
                        Id = uc.user.Id,
                        UserName = uc.user.UserName,
                        Email = uc.user.Email,
                        RoleId = uc.userRole.RoleId,
                        FirstName = uc.user.FirstName,
                        LastName = uc.user.LastName,
                        PhoneNumber = uc.user.PhoneNumber,
                        EmailConfirmed = uc.user.EmailConfirmed
                    })
                    .Distinct()
                    .AsQueryable();
                if (request._filter != null && request._filter.Count > 0)
                {
                    userQuery = MethodExtensions.ApplyFilters(userQuery, request._filter);
                }

                var users = await PagedList<GetPagingUserViewModel>.ToPagedList(userQuery.OrderByDynamic(request._sort, request._order).Distinct().AsNoTracking(), request._start, request._end);
                return new Response<object>(true, new
                {
                    users._start,
                    users._pages,
                    users._end,
                    users._total,
                    users._hasNext,
                    users._hasPrevious,
                    _data = users
                }, "Success");
            }
        }
    }
}
