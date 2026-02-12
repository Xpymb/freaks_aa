using Freaks.Bll.Common.Extensions;
using Freaks.Messages.Common;
using Freaks.Portal.Bll.Implementation.Auction;
using Freaks.Portal.Bll.Implementation.Loot;
using Freaks.Portal.Bll.Implementation.Notification;
using Freaks.Portal.Bll.Implementation.RaidSummary;
using Freaks.Portal.Bll.Implementation.SalarySummary;
using Freaks.Portal.Bll.Implementation.SalarySummary.Mappings;
using Freaks.Portal.Bll.Implementation.Shop;
using Freaks.Portal.Bll.Interfaces.Auction;
using Freaks.Portal.Bll.Interfaces.Loot;
using Freaks.Portal.Bll.Interfaces.Notification;
using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Bll.Interfaces.Shop;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Portal.Bll.Implementation;

/// <summary>
///     Расширения DI для Bll
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Добавить Bll сервисы в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов в DI</param>
    /// <param name="configuration">Конфигурация окружения</param>
    public static IServiceCollection AddBllServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfireCommon(configuration);
        
        // Raid
        services.AddScoped<IRaidAccessService, RaidAccessService>();
        services.AddScoped<IRaidService, RaidService>();
        services.AddScoped<IRaidParticipantService, RaidParticipantService>();
        services.AddScoped<IRaidScreenshotService, RaidScreenshotService>();
        services.AddScoped<IRaidLootService, RaidLootService>();

        // Salary
        services.AddScoped<ISalaryService, SalaryService>();
        services.AddScoped<ISalaryLootService, SalaryLootService>();
        services.AddScoped<ISalaryGuildLeaderService, SalaryGuildLeaderService>();
        services.AddScoped<ISalaryExpensesService, SalaryExpensesService>();
        services.AddScoped<ISalaryFinalReportService, SalaryFinalReportService>();
        services.AddScoped<ISalaryMemberService, SalaryMemberService>();
        services.AddScoped<ISalaryCalculationService, SalaryCalculationService>();
        services.AddScoped<ISalaryStepService, SalaryStepService>();

        // Loot items
        services.AddScoped<ILootItemService, LootItemService>();

        // Shop
        services.AddScoped<IShopItemService, ShopItemService>();
        services.AddScoped<IShopItemRequestService, ShopItemRequestService>();

        // Auction
        services.AddScoped<IAuctionItemService, AuctionItemService>();
        services.AddScoped<IAuctionItemBidService, AuctionItemBidService>();
        
        // Notification
        services.AddScoped<INotificationChannelMessageService, NotificationChannelMessageService>();
        services.AddScoped<INotificationChannelService, NotificationChannelService>();

        // Mapper
        services.AddMapsterCommon(typeof(SalaryGuildLeaderMappingProfile).Assembly);

        // Real-Time messages
        services.AddCentrifugoMessageService(configuration);
        
        return services;
    }
}