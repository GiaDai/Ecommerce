using System;
using System.Collections.Generic;
using Ecommerce.Application.Filters;

namespace Ecommerce.Infrastructure.Identity
{
    public class GetPagingRoleClaimParameter : RequestParameter
    {
        public List<int> id { get; set; }
    }
}
