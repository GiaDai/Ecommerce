using System.Collections.Generic;
using Ecommerce.Application.Filters;

namespace Ecommerce.Infrastructure.Identity.Features.Role.Queries.GetPagingRole
{
    public class GetPagingRoleParameter : RequestParameter
    {
        public List<string> id { get; set; }
    }
}
