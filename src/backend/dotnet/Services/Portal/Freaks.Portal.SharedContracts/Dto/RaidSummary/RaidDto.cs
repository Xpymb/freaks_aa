using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Users.SharedContracts.Dto;

namespace Freaks.Portal.SharedContracts.Dto.RaidSummary;

/// <summary>
///     Полная информация о рейде, включая формат, описание и даты создания/обновления.
///     Используется для детального представления рейдового события.
/// </summary>
/// <param name="Id">Уникальный идентификатор рейда.</param>
/// <param name="BossType">Тип босса, против которого проводится рейд.</param>
/// <param name="FormatType">Формат рейда (может быть null).</param>
/// <param name="Creator">Информация о пользователе, создавшем рейд.</param>
/// <param name="StartDt">Дата и время начала рейда.</param>
/// <param name="CreatedDt">Дата и время создания рейда.</param>
/// <param name="UpdatedDt">Дата и время последнего обновления (может быть null).</param>
/// <param name="Description">Описание рейда и дополнительные заметки.</param>
/// <param name="Status">Текущий статус рейда.</param>
public record RaidDto(
    int Id,
    BossType BossType,
    RaidFormatType? FormatType,
    UserDto Creator,
    DateTimeOffset StartDt,
    DateTimeOffset CreatedDt,
    DateTimeOffset? UpdatedDt,
    string Description,
    RaidStatus Status);