using Freaks.Portal.SharedContracts.Dto.Loot;

namespace Freaks.Portal.Bll.Interfaces.Loot;

/// <summary>
///     Сервис для работы с предметами добычи (лутом).
///     Предоставляет методы бизнес-логики для получения данных о луте.
/// </summary>
public interface ILootItemService
{
    /// <summary>
    ///     Возвращает список всех предметов добычи в виде DTO-моделей.
    /// </summary>
    /// <returns>Список объектов <see cref="LootItemDto" />.</returns>
    Task<IList<LootItemDto>> GetListAsync();
}