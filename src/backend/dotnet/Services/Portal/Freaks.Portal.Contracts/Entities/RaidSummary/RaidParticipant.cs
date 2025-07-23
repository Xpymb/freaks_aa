using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Users.Contracts;

namespace Freaks.Portal.Contracts.Entities.RaidSummary;

/// <summary>
///     Составной ключ для сущности <see cref="RaidParticipant" />.
///     Объединяет идентификаторы рейда и участника, формируя уникальную пару.
/// </summary>
/// <param name="RaidId">Идентификатор рейда.</param>
/// <param name="ParticipantId">Идентификатор участника рейда.</param>
public record RaidParticipantKey(int RaidId, Guid ParticipantId);

/// <summary>
/// Представляет участника рейда в базе данных.
/// Связывает рейд с участником через их идентификаторы.
/// </summary>
[Table("raid_participant", Schema = DatabaseConsts.PortalSchema)]
public class RaidParticipant : ICompositeEntity<RaidParticipantKey>
{
    /// <summary>
    /// Идентификатор рейда, в котором участвует пользователь.
    /// </summary>
    [Column("raid_id")]
    public required int RaidId { get; init; }

    /// <summary>
    /// Идентификатор участника рейда.
    /// </summary>
    [Column("participant_id")]
    public required Guid ParticipantId { get; init; }

    /// <summary>
    ///     Идентификатор пользователя, который создал запись
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    /// Навигационное свойство для доступа к рейду, в котором участвует пользователь.
    /// </summary>
    public Raid? Raid { get; init; }

    /// <summary>
    /// Навигационное свойство для доступа к пользователю-участнику рейда.
    /// </summary>
    public User? Participant { get; init; }

    /// <inheritdoc />
    public RaidParticipantKey GetCompositeKey()
    {
        return new RaidParticipantKey(RaidId, ParticipantId);
    }
}