using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Contracts.ValueObjects.RaidSummary;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.Raid;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Dal.Interfaces.RaidSummary;

/// <summary>
///     Интерфейс провайдера для работы с рейдами в базе данных.
///     Предоставляет методы получения и фильтрации данных о рейдах.
/// </summary>
public interface IRaidProvider : IBaseProvider<Raid, long, IPortalDbContext>
{
    /// <summary>
    ///     Возвращает список рейдов с постраничной выборкой на основе переданных фильтров и параметров сортировки.
    /// </summary>
    /// <param name="request">Параметры фильтрации, сортировки и пагинации.</param>
    /// <returns>Постраничный список рейдов, соответствующих условиям запроса.</returns>
    Task<PaginatedList<Raid>> GetPaginatedListAsync(GetRaidListRequest request);

    Task<IList<RaidFullInfo>> GetFullInfoAsync(DateTimeOffset startDt, DateTimeOffset endDt, IList<BossType> bossTypes);
}
