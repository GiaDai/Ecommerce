using System;
using Ecommerce.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Contexts;

public class ReadDbContext : ApplicationDbContext
{
    public ReadDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options, dateTime, authenticatedUser)
    {
    }
}
