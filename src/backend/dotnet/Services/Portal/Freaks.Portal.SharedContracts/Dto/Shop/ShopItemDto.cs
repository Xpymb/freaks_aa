using Freaks.Portal.SharedContracts.Dto.Loot;
using Freaks.Portal.SharedContracts.ValueObjects.Shop;
using Freaks.Users.SharedContracts.Dto;

namespace Freaks.Portal.SharedContracts.Dto.Shop;

/// <summary>
///     DTO, представляющий информацию о товаре в магазине для отображения на клиенте.
///     Содержит данные о предмете, продавце, стоимости, количестве и статусе.
/// </summary>
/// <param name="Id">Уникальный идентификатор товара в магазине.</param>
/// <param name="LootItem">Связанный предмет лута, который продаётся.</param>
/// <param name="Creator">Пользователь, выставивший товар на продажу.</param>
/// <param name="Price">Цена товара в условных единицах.</param>
/// <param name="TotalQuantity">Общее количество единиц товара.</param>
/// <param name="RemainingQuantity">Оставшееся единиц товара.</param>
/// <param name="Status">Текущий статус товара (активен или завершён).</param>
public record ShopItemDto(
    int Id,
    LootItemDto LootItem,
    UserDto Creator,
    int Price,
    int TotalQuantity,
    int RemainingQuantity,
    ShopItemStatus Status);