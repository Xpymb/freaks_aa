using Freaks.Dal.Common.Extensions;
using Freaks.Portal.Dal.Implementation.Auction;
using Freaks.Portal.Dal.Implementation.Loot;
using Freaks.Portal.Dal.Implementation.RaidSummary;
using Freaks.Portal.Dal.Implementation.Shop;
using Freaks.Portal.Dal.Interfaces.Auction;
using Freaks.Portal.Dal.Interfaces.Loot;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Interfaces.Shop;
using Freaks.Portal.Dal.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Portal.Dal.Implementation;

/// <summary>
///     Расширения DI для Dal
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Добавить Dal сервисы в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов в DI</param>
    /// <param name="configuration">Конфигурация окружения</param>
    public static IServiceCollection AddDalProviders(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgresDbContext<IPortalDbContext, PortalDbContext>(configuration);

        // Raid
        services.AddScoped<IRaidProvider, RaidProvider>();
        services.AddScoped<IRaidParticipantProvider, RaidParticipantProvider>();
        services.AddScoped<IRaidScreenshotProvider, RaidScreenshotProvider>();
        services.AddScoped<IRaidLootProvider, RaidLootProvider>();

        // Loot items
        services.AddScoped<ILootItemProvider, LootItemProvider>();

        // Shop
        services.AddScoped<IShopItemProvider, ShopItemProvider>();
        services.AddScoped<IShopItemRequestProvider, ShopItemRequestProvider>();

        // Auction
        services.AddScoped<IAuctionItemProvider, AuctionItemProvider>();
        services.AddScoped<IAuctionItemBidProvider, AuctionItemBidProvider>();
        
        return services;
    }
}