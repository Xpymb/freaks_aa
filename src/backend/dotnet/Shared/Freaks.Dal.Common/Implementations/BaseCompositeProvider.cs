using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Dal.Common.Implementations;

/// <summary>
///     Базовая реализация провайдера доступа к данным для сущностей с составным ключом.
///     Предоставляет стандартные CRUD-операции и доступ к контексту базы данных.
/// </summary>
/// <typeparam name="TEntity">Тип сущности, реализующий <see cref="ICompositeEntity{TKey}" />.</typeparam>
/// <typeparam name="TKey">Тип составного ключа сущности (чаще всего record).</typeparam>
/// <typeparam name="TDbContext">Тип контекста базы данных, реализующий <see cref="IBaseDbContext" />.</typeparam>
public abstract class BaseCompositeProvider<TEntity, TKey, TDbContext> : IBaseCompositeProvider<TEntity, TKey, TDbContext>
    where TEntity : class, ICompositeEntity<TKey>
    where TDbContext : IBaseDbContext
{
    /// <inheritdoc />
    public DbSet<TEntity> Set { get; }

    /// <inheritdoc />
    public TDbContext DbContext { get; }

    /// <summary>
    ///     Создаёт новый экземпляр <see cref="BaseCompositeProvider{TEntity, TKey, TDbContext}" />.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных, предоставляющий доступ к таблицам и транзакциям.</param>
    /// <exception cref="ArgumentNullException">Если передан <c>null</c> в качестве контекста или DbSet.</exception>
    protected BaseCompositeProvider(TDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Set = dbContext.Set<TEntity>() ?? throw new ArgumentNullException(nameof(DbSet<TEntity>));
    }

    protected abstract IQueryable<TEntity> FilterByKey(TKey key, IQueryable<TEntity> queryable);

    /// <inheritdoc />
    public virtual Task<TEntity?> GetAsync(TKey key, EntityTrackingType trackingType)
    {
        var query = FilterByKey(key, Set);
        if (trackingType is EntityTrackingType.NoTracking)
        {
            query = query.AsNoTracking();
        }

        return query.FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var entry = await Set.AddAsync(entity);
        await DbContext.SaveChangesAsync();

        var result = await GetAsync(entry.Entity.GetCompositeKey(), EntityTrackingType.NoTracking);
        return result!;
    }

    /// <inheritdoc />
    public virtual async Task<IList<TEntity>> SetAsync(IList<TEntity> entities)
    {
        await Set.BulkInsertAsync(
            entities,
            options =>
            {
                options.AutoMapOutputDirection = true;
            });

        return entities;
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var existing = await GetAsync(entity.GetCompositeKey(), EntityTrackingType.Tracking);
        if (existing is null)
        {
            throw new EntityNotFoundException();
        }

        Set.Entry(existing)
           .CurrentValues
           .SetValues(entity);

        await DbContext.SaveChangesAsync();
        
        return existing;
    }

    /// <inheritdoc />
    public virtual async Task<IList<TEntity>> UpdateSetAsync(IList<TEntity> entities)
    {
        await Set.BulkUpdateAsync(entities);
        return entities;
    }

    /// <inheritdoc />
    public virtual Task DeleteAsync(TKey key)
    {
        var query = FilterByKey(key, Set);
        return query.ExecuteDeleteAsync();
    }

    /// <inheritdoc />
    public virtual Task DeleteAsync(TEntity entity)
    {
        var key = entity.GetCompositeKey();
        return DeleteAsync(key);
    }

    /// <inheritdoc />
    public virtual Task DeleteAsync(IList<TKey> keys)
    {
        var query = Set.AsQueryable();

        query =
            keys.Select(key => FilterByKey(key, Set))
                .Aggregate(query, (current, nextQuery) => current.Union(nextQuery));

        return query.ExecuteDeleteAsync();
    }

    /// <inheritdoc />
    public virtual Task DeleteAsync(IList<TEntity> entities)
    {
        var keys =
            entities.Select(key => key.GetCompositeKey())
                    .ToList();

        return DeleteAsync(keys);
    }
}