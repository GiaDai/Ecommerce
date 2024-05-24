namespace Ecommerce.Infrastructure.Identity.Features.Users.Queries.GetPagingUser
{
    public class GetPagingUserViewModel
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
