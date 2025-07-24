using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Loot;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.Loot;

/// <summary>
///     Провайдер для работы с предметами добычи (лутом).
///     Наследует базовые CRUD-операции из <see cref="IBaseProvider{TEntity,TKey,TContext}" />.
/// </summary>
public interface ILootItemProvider : IBaseProvider<LootItem, int, IPortalDbContext>
{
    /// <summary>
    ///     Возвращает список всех предметов добычи.
    /// </summary>
    /// <returns>Список объектов <see cref="LootItem" />.</returns>
    Task<IList<LootItem>> GetListAsync();
}