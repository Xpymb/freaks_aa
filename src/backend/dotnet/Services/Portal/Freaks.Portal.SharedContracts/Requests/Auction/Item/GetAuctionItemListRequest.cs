using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.SharedContracts.ValueObjects.Auction;

namespace Freaks.Portal.SharedContracts.Requests.Auction.Item;

/// <summary>
///     Запрос для получения списка лотов аукциона с возможностью фильтрации, сортировки и постраничной навигации.
/// </summary>
/// <param name="Statuses">Список статусов лотов для фильтрации. Если null, фильтрация по статусам не применяется.</param>
/// <param name="From">
///     Дата и время начала периода для фильтрации лотов. Если null, фильтрация по дате начала не
///     применяется.
/// </param>
/// <param name="To">
///     Дата и время конца периода для фильтрации лотов. Если null, фильтрация по дате окончания не
///     применяется.
/// </param>
/// <param name="SortBy">Поле для сортировки списка лотов.</param>
/// <param name="SortMode">Направление сортировки (по возрастанию или убыванию).</param>
/// <param name="Take">Максимальное число лотов для возврата. Если null, возвращаются все найденные лоты.</param>
/// <param name="Skip">Количество первых лотов, которые нужно пропустить. Если null, пропуск не применяется.</param>
public record GetAuctionItemListRequest(
    List<AuctionItemStatus>? Statuses,
    DateTimeOffset? From,
    DateTimeOffset? To,
    AuctionItemListSortByType SortBy,
    OrderByMode SortMode,
    int? Take,
    int? Skip);