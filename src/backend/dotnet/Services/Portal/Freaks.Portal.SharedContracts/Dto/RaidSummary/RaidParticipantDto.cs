using Freaks.Users.SharedContracts.Dto;

namespace Freaks.Portal.SharedContracts.Dto.RaidSummary;

/// <summary>
///     Представляет участника рейда.
///     Содержит информацию о рейде и пользователе-участнике.
/// </summary>
/// <param name="RaidId">Идентификатор рейда, в котором участвует пользователь.</param>
/// <param name="RaidNumber">Номер рейда, в котором был участник.</param>
/// <param name="RaidPartyNumber">Номер отряда в рейде, в котором был участник.</param>
/// <param name="RaidPartyPositionNumber">Номер позиции в отряде рейда, в котором был участник.</param>
/// <param name="Participant">Информация о пользователе, участвующем в рейде.</param>
public record RaidParticipantDto(
    long RaidId,
    int RaidNumber,
    int RaidPartyNumber,
    int RaidPartyPositionNumber,
    UserDto Participant);