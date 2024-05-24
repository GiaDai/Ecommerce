using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ecommerce.Application.Wrappers;
using Ecommerce.Infrastructure.Identity.Contexts;
using Ecommerce.Infrastructure.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Identity.Features.RoleClaim.Queries.GetPagingRoleClaim
{
    public class GetPagingRoleClaimQuery : IRequest<Response<object>>
    {
        public List<int> id { get; set; }
        public int _start { get; set; }
        public int _end { get; set; }
        public string _sort { get; set; }
        public string _order { get; set; }
        public List<string> _filter { get; set; }

        public class GetPagingRoleClaimHanlder : IRequestHandler<GetPagingRoleClaimQuery, Response<object>>
        {
            private readonly IMapper _mapper;
            private readonly IdentityContext _context;
            public GetPagingRoleClaimHanlder(
                IMapper mapper,
                IdentityContext context
            )
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Response<object>> Handle(GetPagingRoleClaimQuery request, CancellationToken cancellationToken)
            {
                if (request.id != null && request.id.Count > 0)
                {
                    var roleClaimIds = await _context.RoleClaims
                        .AsNoTracking()
                        .Where(x => request.id.Contains(x.Id))
                        .ToListAsync();
                    return new Response<object>(true, roleClaimIds, message: "Success");
                }

                var roleClaimQuery = _context.RoleClaims
                    .AsQueryable();
                if (request._filter != null && request._filter.Count > 0)
                {
                    roleClaimQuery = MethodExtensions.ApplyFilters(roleClaimQuery, request._filter);
                }

                var roleClaims = await PagedList<IdentityRoleClaim<string>>.ToPagedList(roleClaimQuery.OrderByDynamic(request._sort, request._order).AsNoTracking(), request._start, request._end);
                return new Response<object>(true, new
                {
                    roleClaims._start,
                    roleClaims._end,
                    roleClaims._total,
                    roleClaims._hasNext,
                    roleClaims._hasPrevious,
                    roleClaims._pages,
                    _data = roleClaims
                }, message: "Success");
            }
        }
    }
}
