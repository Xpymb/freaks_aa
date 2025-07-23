using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidParticipant;

namespace Freaks.Portal.Bll.Interfaces.RaidSummary;

/// <summary>
///     Сервис для управления участниками рейдов.
///     Предоставляет методы для получения списка участников, добавления и удаления.
/// </summary>
public interface IRaidParticipantService
{
    /// <summary>
    ///     Возвращает список участников указанного рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список участников рейда.</returns>
    Task<IList<RaidParticipantDto>> GetListAsync(int raidId);

    /// <summary>
    ///     Добавляет нового участника в рейд.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда</param>
    /// <param name="request">Запрос с данными участника и рейда.</param>
    /// <returns>Добавленный участник рейда.</returns>
    Task<RaidParticipantDto> CreateAsync(int raidId, CreateRaidParticipantRequest request);

    /// <summary>
    ///     Удаляет участника из рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="participantId">Идентификатор участника (пользователя), которого нужно удалить.</param>
    Task DeleteAsync(int raidId, Guid participantId);
}