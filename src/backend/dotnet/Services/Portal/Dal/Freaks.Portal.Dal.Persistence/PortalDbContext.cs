using System.Reflection;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.Contracts.Entities.Loot;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Users.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Persistence;

/// <summary>
///     Реализация контекста базы данных портала с использованием Entity Framework Core.
///     Предоставляет доступ к наборам сущностей рейдов и добычи боссов.
/// </summary>
public class PortalDbContext : DbContext, IPortalDbContext
{
    /// <summary>
    ///     Создаёт новый экземпляр контекста базы данных портала с указанными опциями.
    /// </summary>
    /// <param name="options">
    ///     Настройки контекста <see cref="PortalDbContext" />,
    ///     включая подключение к базе данных и другие параметры EF Core.
    /// </param>
    public PortalDbContext(DbContextOptions<PortalDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    public DbSet<Raid> Raids { get; init; }

    /// <inheritdoc />
    public DbSet<RaidLoot> RaidLoots { get; init; }

    /// <inheritdoc />
    public DbSet<RaidParticipant> RaidParticipants { get; init; }

    /// <inheritdoc />
    public DbSet<RaidScreenshot> RaidScreenshots { get; init; }

    /// <inheritdoc />
    public DbSet<BossLoot> BossLoots { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<User>()
            .ToTable("users", DatabaseConsts.UsersSchema);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}