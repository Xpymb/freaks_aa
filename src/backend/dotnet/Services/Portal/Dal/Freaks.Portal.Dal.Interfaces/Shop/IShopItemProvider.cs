using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Shop;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.Shop.ShopItem;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Dal.Interfaces.Shop;

/// <summary>
///     Провайдер для работы с предметами магазина.
///     Предоставляет доступ к данным <see cref="ShopItem" /> с возможностью фильтрации по статусу.
///     Наследует стандартные операции из <see cref="IBaseProvider{TEntity,TKey,TContext}" />.
/// </summary>
public interface IShopItemProvider : IBaseProvider<ShopItem, int, IPortalDbContext>
{
    /// <summary>
    ///     Возвращает список предметов магазина с поддержкой фильтрации по статусу и пагинации.
    /// </summary>
    /// <param name="request">Параметры фильтрации и постраничного вывода.</param>
    /// <returns>Список объектов <see cref="ShopItem" />.</returns>
    Task<PaginatedList<ShopItem>> GetListAsync(GetShopItemListRequest request);
}