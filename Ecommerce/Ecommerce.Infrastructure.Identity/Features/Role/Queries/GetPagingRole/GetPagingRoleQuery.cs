using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ecommerce.Application.Wrappers;
using Ecommerce.Infrastructure.Identity.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Infrastructure.Shared.Extensions;

namespace Ecommerce.Infrastructure.Identity.Features.Role.Queries.GetPagingRole
{
    public class GetPagingRoleQuery : IRequest<Response<object>>
    {
        public List<string> id { get; set; }
        public int _start { get; set; }
        public int _end { get; set; }
        public string _sort { get; set; }
        public string _order { get; set; }
        public List<string> _filter { get; set; }

        public class GetPagingUserHanlder : IRequestHandler<GetPagingRoleQuery, Response<object>>
        {
            private readonly IMapper _mapper;
            private readonly IdentityContext _context;
            public GetPagingUserHanlder(
                IMapper mapper,
                IdentityContext context
            )
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Response<object>> Handle(GetPagingRoleQuery request, CancellationToken cancellationToken)
            {
                if (request.id != null && request.id.Count > 0)
                {
                    var roleIds = await _context.Roles
                        .AsNoTracking()
                        .Where(x => request.id.Contains(x.Id))
                        .ToListAsync();
                    return new Response<object>(true, roleIds, message: "Success");
                }

                var roleQuery = _context.Roles
                    .AsQueryable();
                if (request._filter != null && request._filter.Count > 0)
                {
                    roleQuery = MethodExtensions.ApplyFilters(roleQuery, request._filter);
                }

                var roles = await PagedList<IdentityRole>.ToPagedList(roleQuery.OrderByDynamic(request._sort, request._order).AsNoTracking(), request._start, request._end);
                return new Response<object>(true, new
                {
                    roles._start,
                    roles._end,
                    roles._total,
                    roles._hasNext,
                    roles._hasPrevious,
                    roles._pages,
                    _data = roles
                }, message: "Success");
            }
        }
    }
}
