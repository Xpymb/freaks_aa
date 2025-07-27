using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.RaidSummary;

/// <summary>
/// Интерфейс провайдера для управления участниками рейдов.
/// Предоставляет методы для получения, создания, обновления и удаления записей об участниках рейдов.
/// </summary>
public interface IRaidParticipantProvider : IBaseCompositeProvider<RaidParticipant, RaidParticipantKey, IPortalDbContext>
{
    /// <summary>
    /// Возвращает список участников указанного рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список участников рейда.</returns>
    Task<IList<RaidParticipant>> GetByRaidIdAsync(long raidId);

    /// <summary>
    /// Возвращает список рейдов, в которых участвует указанный пользователь.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список участий пользователя в рейдах.</returns>
    Task<IList<RaidParticipant>> GetByUserIdAsync(Guid userId);
}