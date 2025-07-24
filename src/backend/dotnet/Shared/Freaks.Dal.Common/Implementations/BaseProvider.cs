using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Dal.Common.Implementations;

/// <summary>
///     Базовая реализация провайдера доступа к данным.
///     Предоставляет стандартные CRUD-операции и доступ к контексту базы данных.
/// </summary>
/// <typeparam name="TEntity">Тип сущности, реализующий <see cref="IEntity{TKey}" />.</typeparam>
/// <typeparam name="TKey">Тип ключа сущности (например, <c>int</c>, <c>Guid</c> и т.д.).</typeparam>
/// <typeparam name="TDbContext">Тип контекста базы данных, реализующий <see cref="IBaseDbContext" />.</typeparam>
public abstract class BaseProvider<TEntity, TKey, TDbContext> : IBaseProvider<TEntity, TKey, TDbContext>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : IBaseDbContext
{
    /// <inheritdoc />
    public DbSet<TEntity> Set { get; }

    /// <inheritdoc />
    public TDbContext DbContext { get; }

    /// <summary>
    ///     Создаёт новый экземпляр <see cref="BaseProvider{TEntity, TKey, TDbContext}" />.
    /// </summary>
    /// <param name="dbContext">Экземпляр контекста базы данных.</param>
    protected BaseProvider(TDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Set = dbContext.Set<TEntity>() ?? throw new ArgumentNullException(nameof(DbSet<TEntity>));
    }

    /// <inheritdoc />
    public virtual async Task<TEntity?> GetAsync(TKey key, EntityTrackingType trackingType)
    {
        var query = Set.Where(e => e.Id.Equals(key));

        if (trackingType is EntityTrackingType.NoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var resultEntry = await Set.AddAsync(entity);
        await DbContext.SaveChangesAsync();

        return resultEntry.Entity;
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
        var originalEntity = await Set.FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));
        if (originalEntity is null)
        {
            throw new EntityNotFoundException();
        }

        Set
            .Entry(originalEntity)
            .CurrentValues
            .SetValues(entity);

        await DbContext.SaveChangesAsync();

        return originalEntity;
    }

    /// <inheritdoc />
    public virtual async Task<IList<TEntity>> UpdateSetAsync(IList<TEntity> entities)
    {
        await Set.BulkUpdateAsync(entities);
        return entities;
    }

    /// <inheritdoc />
    public virtual async Task DeleteAsync(TKey key)
    {
        await Set
              .Where(e => e.Id.Equals(key))
              .ExecuteDeleteAsync();
    }

    /// <inheritdoc />
    public virtual async Task DeleteAsync(TEntity entity)
    {
        await DeleteAsync(entity.Id);
    }

    /// <inheritdoc />
    public virtual async Task DeleteAsync(IList<TKey> keys)
    {
        await Set
              .Where(e => keys.Contains(e.Id))
              .ExecuteDeleteAsync();
    }

    /// <inheritdoc />
    public virtual async Task DeleteAsync(IList<TEntity> entities)
    {
        var ids =
            entities
                .Select(e => e.Id)
                .ToList();

        await DeleteAsync(ids);
    }
}