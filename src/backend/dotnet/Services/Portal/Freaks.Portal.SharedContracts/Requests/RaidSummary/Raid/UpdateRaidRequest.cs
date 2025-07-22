using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;

namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.Raid;

/// <summary>
///     Запрос на обновление информации о рейде.
///     Содержит новые значения для типа босса, формата и описания.
/// </summary>
/// <param name="Id">Идентификатор рейда, который необходимо обновить.</param>
/// <param name="BossType">Новый тип босса.</param>
/// <param name="FormatType">Новый формат рейда.</param>
/// <param name="Description">Обновлённое описание рейда.</param>
public record UpdateRaidRequest(
    int Id,
    BossType BossType,
    RaidFormatType FormatType,
    string Description);