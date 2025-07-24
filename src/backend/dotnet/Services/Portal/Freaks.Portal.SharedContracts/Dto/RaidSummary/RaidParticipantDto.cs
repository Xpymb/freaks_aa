using Freaks.Users.SharedContracts;

namespace Freaks.Portal.SharedContracts.Dto.RaidSummary;

/// <summary>
///     Представляет участника рейда.
///     Содержит информацию о рейде и пользователе-участнике.
/// </summary>
/// <param name="RaidId">Идентификатор рейда, в котором участвует пользователь.</param>
/// <param name="Participant">Информация о пользователе, участвующем в рейде.</param>
public record RaidParticipantDto(
    int RaidId,
    UserDto Participant);