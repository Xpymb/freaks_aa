using Freaks.Portal.SharedContracts.Dto.Shop;
using Freaks.Portal.SharedContracts.Requests.Shop.ShopItemRequest;

namespace Freaks.Portal.Bll.Interfaces.Shop;

/// <summary>
///     Интерфейс сервиса для работы с заявками пользователей на покупку товаров из магазина.
///     Предоставляет методы для получения списка заявок, создания новой заявки, обновления существующей и удаления.
/// </summary>
public interface IShopItemRequestService
{
    /// <summary>
    ///     Получает список заявок на указанный товар магазина.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара магазина.</param>
    /// <returns>Список DTO товаров, на которые оставлены заявки.</returns>
    Task<IList<ShopItemRequestDto>> GetListAsync(int shopItemId);

    /// <summary>
    ///     Создаёт новую заявку пользователя на указанный товар.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара магазина.</param>
    /// <param name="request">Данные для создания заявки.</param>
    /// <returns>Созданный DTO заявки на товар.</returns>
    Task<ShopItemRequestDto> CreateAsync(int shopItemId, CreateShopItemModelRequest request);

    /// <summary>
    ///     Обновляет существующую заявку на указанный товар.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара магазина.</param>
    /// <param name="request">Данные для обновления заявки.</param>
    /// <returns>Обновлённый DTO заявки на товар.</returns>
    Task<ShopItemRequestDto> UpdateStatusAsync(int shopItemId, UpdateStatusShopItemRequest request);

    /// <summary>
    ///     Удаляет заявку пользователя на указанный товар.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара магазина.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>DTO удалённой заявки на товар.</returns>
    Task DeleteAsync(int shopItemId, Guid userId);
}