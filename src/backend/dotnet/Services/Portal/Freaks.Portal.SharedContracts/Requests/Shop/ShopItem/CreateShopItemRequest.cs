namespace Freaks.Portal.SharedContracts.Requests.Shop.ShopItem;

/// <summary>
///     Запрос на создание нового товара в магазине.
///     Содержит информацию о предмете, цене и количестве.
/// </summary>
/// <param name="LootItemId">Идентификатор предмета лута, который будет выставлен в магазин.</param>
/// <param name="Price">Цена товара в условных единицах.</param>
/// <param name="Quantity">Количество единиц товара, доступных для продажи.</param>
public record CreateShopItemRequest(
    int LootItemId,
    int Price,
    int Quantity);