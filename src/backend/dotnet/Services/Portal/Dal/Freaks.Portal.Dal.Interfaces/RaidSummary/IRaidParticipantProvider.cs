using Freaks.Portal.Contracts.Entities.RaidSummary;

namespace Freaks.Portal.Dal.Interfaces.RaidSummary;

/// <summary>
/// Интерфейс провайдера для управления участниками рейдов.
/// Предоставляет методы для получения, создания, обновления и удаления записей об участниках рейдов.
/// </summary>
public interface IRaidParticipantProvider
{
    /// <summary>
    /// Возвращает список участников указанного рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список участников рейда.</returns>
    Task<IList<RaidParticipant>> GetByRaidIdAsync(int raidId);

    /// <summary>
    /// Возвращает список рейдов, в которых участвует указанный пользователь.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список участий пользователя в рейдах.</returns>
    Task<IList<RaidParticipant>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Создаёт новую запись об участии пользователя в рейде.
    /// </summary>
    /// <param name="participant">Объект участника рейда.</param>
    /// <returns>Созданная сущность участника.</returns>
    Task<RaidParticipant> CreateAsync(RaidParticipant participant);

    /// <summary>
    /// Обновляет информацию об участнике рейда.
    /// </summary>
    /// <param name="participant">Обновлённый объект участника рейда.</param>
    /// <returns>Обновлённая сущность участника.</returns>
    Task<RaidParticipant> UpdateAsync(RaidParticipant participant);

    /// <summary>
    /// Удаляет участника из рейда.
    /// </summary>
    /// <param name="participant">Участник рейда, подлежащий удалению.</param>
    Task DeleteAsync(RaidParticipant participant);
}