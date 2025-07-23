using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Dal.Common.Interfaces;

/// <summary>
///     Базовый интерфейс для провайдеров доступа к данным сущностей с составным (композитным) ключом.
///     Предоставляет стандартные CRUD-операции и доступ к <see cref="DbContext" /> и <see cref="DbSet{TEntity}" />.
///     Используется как контракт для реализации репозиториев, работающих с сущностями, у которых уникальный идентификатор
///     состоит из нескольких полей.
/// </summary>
/// <typeparam name="TEntity">Тип сущности, реализующий <see cref="ICompositeEntity{TKey}" />.</typeparam>
/// <typeparam name="TKey">Тип составного ключа сущности (обычно record или структура).</typeparam>
/// <typeparam name="TDbContext">Тип контекста базы данных, реализующий <see cref="IBaseDbContext" />.</typeparam>
public interface IBaseCompositeProvider<TEntity, TKey, out TDbContext>
    where TEntity : class, ICompositeEntity<TKey>
    where TDbContext : IBaseDbContext
{
    /// <summary>
    ///     Набор сущностей <see cref="DbSet{TEntity}" />, ассоциированных с текущим контекстом данных.
    ///     Используется для построения LINQ-запросов и манипуляций с данными.
    /// </summary>
    DbSet<TEntity> Set { get; }

    /// <summary>
    ///     Экземпляр контекста базы данных, используемый текущим провайдером.
    ///     Предоставляет доступ к операциям сохранения, транзакциям, SQL-запросам и отслеживанию изменений.
    /// </summary>
    TDbContext DbContext { get; }

    /// <summary>
    ///     Получает сущность по составному ключу с возможностью указания режима отслеживания изменений.
    /// </summary>
    /// <param name="key">Составной ключ искомой сущности.</param>
    /// <param name="trackingType">Режим отслеживания: <c>Tracking</c> или <c>NoTracking</c>.</param>
    /// <returns>Найденная сущность или <c>null</c>, если не найдена.</returns>
    Task<TEntity?> GetAsync(TKey key, EntityTrackingType trackingType);

    /// <summary>
    ///     Создаёт новую сущность в базе данных.
    /// </summary>
    /// <param name="entity">Создаваемая сущность.</param>
    /// <returns>Созданная сущность.</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    ///     Массовое создание сущностей в базе данных.
    /// </summary>
    /// <param name="entities">Список сущностей для создания.</param>
    /// <returns>Список успешно созданных сущностей.</returns>
    Task<IList<TEntity>> SetAsync(IList<TEntity> entities);

    /// <summary>
    ///     Обновляет существующую сущность в базе данных.
    /// </summary>
    /// <param name="entity">Обновляемая сущность.</param>
    /// <returns>Обновлённая сущность.</returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    ///     Массовое обновление сущностей в базе данных.
    /// </summary>
    /// <param name="entities">Список сущностей для обновления.</param>
    /// <returns>Список успешно обновлённых сущностей.</returns>
    Task<IList<TEntity>> UpdateSetAsync(IList<TEntity> entities);

    /// <summary>
    ///     Удаляет сущность по составному ключу.
    /// </summary>
    /// <param name="key">Составной ключ удаляемой сущности.</param>
    Task DeleteAsync(TKey key);

    /// <summary>
    ///     Удаляет сущность.
    /// </summary>
    /// <param name="entity">Сущность для удаления.</param>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    ///     Удаляет несколько сущностей по списку ключей.
    /// </summary>
    /// <param name="keys">Список уникальных идентификаторов сущностей для удаления.</param>
    Task DeleteAsync(IList<TKey> keys);

    /// <summary>
    ///     Удаляет несколько сущностей.
    /// </summary>
    /// <param name="entities">Список сущностей для удаления.</param>
    Task DeleteAsync(IList<TEntity> entities);
}