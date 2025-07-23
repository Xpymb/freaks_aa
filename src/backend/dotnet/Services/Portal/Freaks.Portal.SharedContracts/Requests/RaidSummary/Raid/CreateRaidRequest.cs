using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;

namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.Raid;

/// <summary>
///     Запрос на создание нового рейда.
///     Содержит базовую информацию, необходимую для создания: тип босса, дату начала и описание.
/// </summary>
/// <param name="BossType">Тип босса, против которого будет проводиться рейд.</param>
/// <param name="StartDt">Дата и время начала рейда.</param>
/// <param name="Description">Описание рейда и дополнительные заметки.</param>
public record CreateRaidRequest(
    BossType BossType,
    DateTimeOffset StartDt,
    string Description);