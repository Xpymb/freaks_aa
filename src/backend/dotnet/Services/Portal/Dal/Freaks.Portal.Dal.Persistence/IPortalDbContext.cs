using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Auction;
using Freaks.Portal.Contracts.Entities.Loot;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Contracts.Entities.Shop;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Persistence;

/// <summary>
///     Интерфейс контекста базы данных портала.
///     Определяет наборы сущностей для работы с рейдами и добычей боссов
///     через Entity Framework.
/// </summary>
public interface IPortalDbContext : IBaseDbContext
{
    /// <summary>
    ///     Набор рейдов.
    /// </summary>
    DbSet<Raid> Raids { get; }

    /// <summary>
    ///     Набор добычи рейдов.
    /// </summary>
    DbSet<RaidLoot> RaidLoots { get; }

    /// <summary>
    ///     Набор участников рейдов.
    /// </summary>
    DbSet<RaidParticipant> RaidParticipants { get; }

    /// <summary>
    ///     Набор скриншотов рейдов.
    /// </summary>
    DbSet<RaidScreenshot> RaidScreenshots { get; }

    /// <summary>
    ///     Набор возможной добычи от мировых боссов.
    /// </summary>
    DbSet<LootItem> LootItems { get; }

    /// <summary>
    ///     Набор предметов магазина, доступных для покупки.
    /// </summary>
    DbSet<ShopItem> ShopItems { get; }

    /// <summary>
    ///     Набор заявок на покупку предметов магазина.
    /// </summary>
    DbSet<ShopItemRequest> ShopItemRequests { get; }

    /// <summary>
    ///     Набор сущностей аукционных лотов.
    /// </summary>
    DbSet<AuctionItem> AuctionItems { get; }

    /// <summary>
    ///     Набор сущностей ставок по лотам аукциона.
    /// </summary>
    DbSet<AuctionItemBid> AuctionItemBids { get; }
    
    /// <summary>
    ///    Набор чатов Discord сервера.
    /// </summary>
    DbSet<NotificationChannel> NotificationChannels { get; }
    
    /// <summary>
    ///    Набор сообщений отправляемых пользователями.
    /// </summary>
    DbSet<NotificationChannelMessage> NotificationChannelMessages { get; }

    /// <summary>
    ///    Набор зарплатных отчётов.
    /// </summary>
    DbSet<Salary> Salaries { get; }

    /// <summary>
    ///    Набор параметров зарплатных отчётов.
    /// </summary>
    DbSet<SalaryParameters> SalaryParameters { get; }

    /// <summary>
    ///    Набор участников зарплатных отчётов.
    /// </summary>
    DbSet<SalaryMember> SalaryMembers { get; }

    /// <summary>
    ///    Набор проданных предметов зарплатного отчёта.
    /// </summary>
    DbSet<SalaryLoot> SalaryLoots { get; }

    /// <summary>
    ///    Набор доли гильд лидера в зарплатных отчётах.
    /// </summary>
    DbSet<SalaryGuildLeader> SalaryGuildLeaders { get; }

    /// <summary>
    ///    Набор расходов в зарплатных отчётах.
    /// </summary>
    DbSet<SalaryExpenses> SalaryExpenses { get; }
}