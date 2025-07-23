using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Users.Contracts;

namespace Freaks.Portal.SharedContracts.Dto.RaidSummary;

/// <summary>
///     Краткое представление рейда для отображения в списках или превью.
/// </summary>
/// <param name="Id">Уникальный идентификатор рейда.</param>
/// <param name="BossType">Тип босса, против которого проводится рейд.</param>
/// <param name="Creator">Информация о пользователе, создавшем рейд.</param>
/// <param name="StartDt">Дата и время начала рейда.</param>
/// <param name="Status">Текущий статус рейда.</param>
public record RaidShortDto(
    int Id,
    BossType BossType,
    UserDto Creator,
    DateTimeOffset StartDt,
    RaidStatus Status);