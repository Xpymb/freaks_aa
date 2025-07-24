using Freaks.Portal.SharedContracts.Dto.Shop;
using Freaks.Portal.SharedContracts.Requests.Shop.ShopItem;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Bll.Interfaces.Shop;

/// <summary>
///     Сервис для работы с товарами магазина.
///     Предоставляет методы для получения, создания, обновления и удаления товаров,
///     а также получения списка с поддержкой фильтрации и пагинации.
/// </summary>
public interface IShopItemService
{
    /// <summary>
    ///     Получает информацию о товаре по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор товара.</param>
    /// <returns>Объект <see cref="ShopItemDto" /> с деталями товара.</returns>
    Task<ShopItemDto> GetAsync(int id);

    /// <summary>
    ///     Возвращает список товаров с поддержкой фильтрации по статусу и постраничного вывода.
    /// </summary>
    /// <param name="request">Параметры фильтрации и пагинации.</param>
    /// <returns>Пагинированный список объектов <see cref="ShopItemDto" />.</returns>
    Task<PaginatedList<ShopItemDto>> GetListAsync(GetShopItemListRequest request);

    /// <summary>
    ///     Создаёт новый товар в магазине.
    /// </summary>
    /// <param name="request">Данные для создания товара.</param>
    /// <returns>Созданный объект <see cref="ShopItemDto" />.</returns>
    Task<ShopItemDto> CreateAsync(CreateShopItemRequest request);

    /// <summary>
    ///     Обновляет существующий товар в магазине.
    /// </summary>
    /// <param name="id">Идентификатор товара в магазине.</param>
    /// <param name="request">Данные для обновления товара.</param>
    /// <returns>Обновлённый объект <see cref="ShopItemDto" />.</returns>
    Task<ShopItemDto> UpdateAsync(int id, UpdateShopItemRequest request);

    /// <summary>
    ///     Удаляет товар из магазина по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемого товара.</param>
    Task DeleteAsync(int id);
}