using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Loot;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Persistence;

/// <summary>
/// Интерфейс контекста базы данных портала.
/// Определяет наборы сущностей для работы с рейдами и добычей боссов
/// через Entity Framework.
/// </summary>
public interface IPortalDbContext : IBaseDbContext
{
    /// <summary>
    /// Набор рейдов.
    /// </summary>
    DbSet<Raid> Raids { get; }

    /// <summary>
    /// Набор добычи рейдов.
    /// </summary>
    DbSet<RaidLoot> RaidLoots { get; }

    /// <summary>
    /// Набор участников рейдов.
    /// </summary>
    DbSet<RaidParticipant> RaidParticipants { get; }

    /// <summary>
    /// Набор скриншотов рейдов.
    /// </summary>
    DbSet<RaidScreenshot> RaidScreenshots { get; }

    /// <summary>
    /// Набор возможной добычи от мировых боссов.
    /// </summary>
    DbSet<BossLoot> BossLoots { get; }
}