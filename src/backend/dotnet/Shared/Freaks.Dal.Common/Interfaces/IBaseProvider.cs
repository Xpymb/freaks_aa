using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Dal.Common.Interfaces;

/// <summary>
///     Базовый интерфейс провайдера доступа к сущностям.
///     Предоставляет стандартные операции CRUD с поддержкой трекинга, пакетной обработки и доступом к контексту базы
///     данных.
/// </summary>
/// <typeparam name="TEntity">Тип сущности, реализующий <see cref="IEntity{TKey}" />.</typeparam>
/// <typeparam name="TKey">Тип уникального идентификатора сущности (например, <c>int</c>, <c>Guid</c> и т.д.).</typeparam>
/// <typeparam name="TDbContext">Тип контекста базы данных, реализующий <see cref="IBaseDbContext" />.</typeparam>
public interface IBaseProvider<TEntity, TKey, out TDbContext>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext, IBaseDbContext
{
    /// <summary>
    ///     Набор сущностей <see cref="DbSet{TEntity}" />, ассоциированных с текущим контекстом данных.
    ///     Используется для построения запросов и манипуляций с данными.
    /// </summary>
    DbSet<TEntity> Set { get; }

    /// <summary>
    ///     Экземпляр контекста базы данных, используемый текущим провайдером.
    ///     Предоставляет доступ к транзакциям, SQL-командам, ChangeTracker и т.д.
    /// </summary>
    TDbContext DbContext { get; }

    /// <summary>
    ///     Получает сущность по ключу с возможностью указания режима отслеживания изменений.
    /// </summary>
    /// <param name="key">Уникальный идентификатор сущности.</param>
    /// <param name="trackingType">Режим отслеживания: <c>Tracking</c> или <c>NoTracking</c>.</param>
    /// <returns>Найденная сущность или <c>null</c>, если не найдена.</returns>
    Task<TEntity?> GetAsync(TKey key, EntityTrackingType trackingType);

    /// <summary>
    ///     Создаёт новую сущность в базе данных.
    /// </summary>
    /// <param name="entity">Создаваемая сущность.</param>
    /// <returns>Созданная сущность с заполненными полями (например, идентификатором).</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    ///     Создаёт несколько сущностей в базе данных.
    /// </summary>
    /// <param name="entities">Список сущностей для создания.</param>
    /// <returns>Список созданных сущностей.</returns>
    Task<IList<TEntity>> SetAsync(IList<TEntity> entities);

    /// <summary>
    ///     Обновляет существующую сущность в базе данных.
    /// </summary>
    /// <param name="entity">Обновляемая сущность.</param>
    /// <returns>Обновлённая сущность.</returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    ///     Обновляет несколько сущностей в базе данных.
    /// </summary>
    /// <param name="entities">Список сущностей для обновления.</param>
    /// <returns>Список обновлённых сущностей.</returns>
    Task<IList<TEntity>> UpdateSetAsync(IList<TEntity> entities);

    /// <summary>
    ///     Удаляет сущность по её уникальному идентификатору.
    /// </summary>
    /// <param name="key">Ключ удаляемой сущности.</param>
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