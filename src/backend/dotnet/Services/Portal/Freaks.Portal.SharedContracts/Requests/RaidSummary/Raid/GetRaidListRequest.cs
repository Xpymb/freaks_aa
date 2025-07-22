using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;

namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.Raid;

/// <summary>
///     Запрос на получение списка рейдов с фильтрацией, сортировкой и указанием диапазона дат.
///     Используется для построения списка рейдов в соответствии с выбранными параметрами.
/// </summary>
/// <param name="BossTypes">Список типов боссов, по которым нужно фильтровать рейды.</param>
/// <param name="FormatTypes">Список форматов рейдов для фильтрации.</param>
/// <param name="Statuses">Список статусов рейдов для фильтрации.</param>
/// <param name="From">Начальная дата диапазона (включительно), начиная с которой искать рейды.</param>
/// <param name="To">Конечная дата диапазона (включительно), до которой искать рейды.</param>
/// <param name="SortBy">Поле, по которому нужно отсортировать результаты.</param>
/// <param name="SortMode">Направление сортировки (по возрастанию или убыванию).</param>
public record GetRaidListRequest(
    List<BossType> BossTypes,
    List<RaidFormatType> FormatTypes,
    List<RaidStatus> Statuses,
    DateTimeOffset? From,
    DateTimeOffset? To,
    RaidListSortByType SortBy,
    OrderByMode SortMode,
    int? Take,
    int? Skip);