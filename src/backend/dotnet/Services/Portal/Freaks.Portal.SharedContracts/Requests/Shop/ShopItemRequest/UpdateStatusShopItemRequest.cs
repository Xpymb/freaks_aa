using Freaks.Portal.SharedContracts.ValueObjects.Shop;

namespace Freaks.Portal.SharedContracts.Requests.Shop.ShopItemRequest;

/// <summary>
///     Запрос на обновление статуса товара магазина.
///     Используется для смены состояния, например, с <c>Active</c> на <c>Ended</c>.
/// </summary>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="Status">Новый статус заявки на покупку товара в магазине.</param>
public record UpdateStatusShopItemRequest(
    Guid UserId,
    ShopItemRequestStatus Status);