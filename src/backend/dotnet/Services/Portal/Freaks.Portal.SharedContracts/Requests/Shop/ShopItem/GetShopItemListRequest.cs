using Freaks.Portal.SharedContracts.ValueObjects.Shop;

namespace Freaks.Portal.SharedContracts.Requests.Shop.ShopItem;

/// <summary>
///     Запрос на получение списка предметов магазина с возможностью фильтрации и пагинации.
/// </summary>
/// <param name="Status">Необязательный фильтр по статусу товара (например, только активные).</param>
/// <param name="Take">Количество элементов для выборки (лимит).</param>
/// <param name="Skip">Количество элементов для пропуска (offset).</param>
public record GetShopItemListRequest(
    ShopItemStatus? Status,
    int? Take,
    int? Skip);