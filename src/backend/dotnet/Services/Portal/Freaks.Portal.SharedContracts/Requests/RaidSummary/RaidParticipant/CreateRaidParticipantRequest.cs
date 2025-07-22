namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidParticipant;

/// <summary>
///     Запрос на добавление участника в рейд.
///     Содержит идентификатор рейда и участника.
/// </summary>
/// <param name="RaidId">Идентификатор рейда, в который добавляется участник.</param>
/// <param name="ParticipantId">Идентификатор пользователя, которого нужно добавить как участника.</param>
public record CreateRaidParticipantRequest(
    int RaidId,
    Guid ParticipantId);