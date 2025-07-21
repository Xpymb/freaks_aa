using Freaks.Portal.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.Persistence;

public class PortalDbContext : DbContext, IPortalDbContext
{
    public PortalDbContext(DbContextOptions<PortalDbContext> options)
        : base(options)
    {
    }
}