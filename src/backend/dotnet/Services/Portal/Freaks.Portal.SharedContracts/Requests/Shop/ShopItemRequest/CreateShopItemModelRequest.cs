namespace Freaks.Portal.SharedContracts.Requests.Shop.ShopItemRequest;

/// <summary>
///     Запрос на заявку в магазине
/// </summary>
/// <param name="Quantity">Количество товара</param>
public record CreateShopItemModelRequest(int Quantity);