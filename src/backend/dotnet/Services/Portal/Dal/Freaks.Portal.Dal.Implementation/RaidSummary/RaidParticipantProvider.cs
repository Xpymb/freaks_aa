using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.RaidSummary;

/// <summary>
/// Провайдер для работы с участниками рейдов (<see cref="RaidParticipant"/>), включая кэширование по рейдам и пользователям.
/// </summary>
public class RaidParticipantProvider : IRaidParticipantProvider
{
    private readonly ICacheProvider _cacheProvider;
    private readonly IPortalDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RaidParticipantProvider"/>.
    /// </summary>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    public RaidParticipantProvider(
        ICacheProvider cacheProvider,
        IPortalDbContext dbContext)
    {
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public async Task<IList<RaidParticipant>> GetByRaidIdAsync(int raidId)
    {
        var cacheKey = GetCacheRaidKey(raidId);
        var cachedValue = await _cacheProvider.GetAsync<IList<RaidParticipant>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await _dbContext.RaidParticipants
                                     .Include(p => p.Participant)
                                     .AsNoTracking()
                                     .Where(p => p.RaidId == raidId)
                                     .ToListAsync();

        await _cacheProvider.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<IList<RaidParticipant>> GetByUserIdAsync(Guid userId)
    {
        var cacheKey = GetCacheUserKey(userId);
        var cachedValue = await _cacheProvider.GetAsync<IList<RaidParticipant>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await _dbContext.RaidParticipants
                                     .Include(p => p.Participant)
                                     .AsNoTracking()
                                     .Where(p => p.ParticipantId == userId)
                                     .ToListAsync();

        await _cacheProvider.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<RaidParticipant> CreateAsync(RaidParticipant participant)
    {
        await RemoveCacheAsync(participant);

        var entry = await _dbContext.RaidParticipants.AddAsync(participant);
        await _dbContext.SaveChangesAsync();

        return entry.Entity;
    }

    /// <inheritdoc />
    public async Task<RaidParticipant> UpdateAsync(RaidParticipant participant)
    {
        await RemoveCacheAsync(participant);

        var entry = _dbContext.RaidParticipants.Entry(participant);
        entry.CurrentValues.SetValues(participant);
        await _dbContext.SaveChangesAsync();

        return entry.Entity;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(RaidParticipant participant)
    {
        await RemoveCacheAsync(participant);

        await _dbContext.RaidParticipants
                        .Where(p => p.RaidId == participant.RaidId && p.ParticipantId == participant.ParticipantId)
                        .ExecuteDeleteAsync();
    }

    /// <summary>
    /// Удаляет все связанные с участником ключи кэша (по рейду и пользователю).
    /// </summary>
    /// <param name="participant">Участник рейда, к которому относится кэш.</param>
    private async Task RemoveCacheAsync(RaidParticipant participant)
    {
        var allCacheKeys = GetAllCacheKeys(participant);
        await _cacheProvider.RemoveAsync(allCacheKeys);
    }

    /// <summary>
    /// Генерирует ключ кэша для списка участников конкретного рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheRaidKey(int raidId)
    {
        return $"{nameof(RaidParticipant)}:list:raid:{raidId}";
    }

    /// <summary>
    /// Генерирует ключ кэша для списка участий конкретного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheUserKey(Guid userId)
    {
        return $"{nameof(RaidParticipant)}:list:user:{userId}";
    }

    /// <summary>
    /// Возвращает список всех ключей кэша, связанных с участником рейда.
    /// </summary>
    /// <param name="participant">Участник рейда.</param>
    /// <returns>Список строковых ключей кэша.</returns>
    private static List<string> GetAllCacheKeys(RaidParticipant participant)
    {
        return
        [
            GetCacheRaidKey(participant.RaidId),
            GetCacheUserKey(participant.ParticipantId),
        ];
    }
}