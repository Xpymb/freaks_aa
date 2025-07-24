using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Shop;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.Shop;

/// <summary>
///     Провайдер для работы с заявками на покупку товаров из магазина.
///     Предоставляет доступ к данным <see cref="ShopItemRequest" /> и реализует методы получения записей.
///     Наследует базовые операции из <see cref="IBaseCompositeProvider{TEntity,TKey,TContext}" />.
/// </summary>
public interface IShopItemRequestProvider : IBaseCompositeProvider<ShopItemRequest, ShopItemRequestKey, IPortalDbContext>
{
    /// <summary>
    ///     Возвращает список заявок на указанный товар.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара магазина.</param>
    /// <returns>Список заявок <see cref="ShopItemRequest" /> для указанного товара.</returns>
    Task<IList<ShopItemRequest>> GetListAsync(int shopItemId);
}