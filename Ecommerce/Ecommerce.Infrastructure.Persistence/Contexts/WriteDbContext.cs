using System;
using Ecommerce.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Contexts;

public class WriteDbContext : ApplicationDbContext
{
    public WriteDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options, dateTime, authenticatedUser)
    {
    }
}
