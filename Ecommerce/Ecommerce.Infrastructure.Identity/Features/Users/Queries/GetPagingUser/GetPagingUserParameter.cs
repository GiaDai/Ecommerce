using Ecommerce.Application.Filters;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Identity.Features.Users.Queries.GetPagingUser
{
    public class GetPagingUserParameter : RequestParameter
    {
        public List<string> id { get; set; }
    }
}
