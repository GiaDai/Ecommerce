using System;
using FluentValidation;

namespace Ecommerce.Infrastructure.Identity.Features.RoleClaim.Commands.CreateRoleClaim
{
    public class CreateRoleClaimCommandValidator : AbstractValidator<CreateRoleClaimCommand>
    {
        public CreateRoleClaimCommandValidator()
        {
            RuleFor(v => v.RoleId)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v => v.ClaimType)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v => v.ClaimValue)
                .NotEmpty();

        }
    }
}
