namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidParticipant;

/// <summary>
///     Запрос на добавление участника в рейд.
///     Содержит идентификатор рейда и участника.
/// </summary>
/// <param name="ParticipantId">Идентификатор пользователя, которого нужно добавить как участника.</param>
/// <param name="RaidNumber">Номер рейда, в котором был участник.</param>
/// <param name="RaidPartyNumber">Номер отряда в рейде, в котором был участник.</param>
/// <param name="RaidPartyPositionNumber">Номер позиции в отряде рейда, в котором был участник.</param>
public record CreateRaidParticipantRequest(
    Guid ParticipantId,
    int RaidNumber,
    int RaidPartyNumber,
    int RaidPartyPositionNumber);