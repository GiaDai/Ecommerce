using System;
using Ecommerce.Infrastructure.Identity.Models;

namespace Ecommerce.Infrastructure.Identity.Features.Users.Queries.GetUserById
{
    public class GetUserByIdModel : ApplicationUser
    {
        public UserAvatarClaim Avatar { get; set; }
    }
}
