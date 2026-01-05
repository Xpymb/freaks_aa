namespace Freaks.Portal.SharedContracts.ValueObjects.Shop;

/// <summary>
///     Статус заявки на товар в магазине
/// </summary>
public enum ShopItemRequestStatus
{
    /// <summary>
    ///     Ожидает подтверждения
    /// </summary>
    WaitingSubmit = 1,

    /// <summary>
    ///     Принята
    /// </summary>
    Submitted = 2,

    /// <summary>
    ///     Отклонена
    /// </summary>
    Declined = 3,
}