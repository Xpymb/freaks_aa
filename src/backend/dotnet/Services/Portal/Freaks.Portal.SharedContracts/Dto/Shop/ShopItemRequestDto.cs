using Freaks.Portal.SharedContracts.ValueObjects.Shop;
using Freaks.Users.SharedContracts;

namespace Freaks.Portal.SharedContracts.Dto.Shop;

/// <summary>
///     DTO, представляющий заявку пользователя на покупку товара из магазина.
///     Используется для передачи информации о заявке между слоями приложения.
/// </summary>
/// <param name="ShopItemId">Идентификатор товара магазина, на который оставлена заявка.</param>
/// <param name="User">Пользователь, оставивший заявку.</param>
/// <param name="Quantity">Количество товара.</param>
/// <param name="Status">Текущий статус заявки.</param>
public record ShopItemRequestDto(
    int ShopItemId,
    UserDto User,
    int Quantity,
    ShopItemRequestStatus Status);