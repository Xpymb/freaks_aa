using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Dal.Common.Consts;
using Freaks.Users.Contracts;

namespace Freaks.Portal.Contracts.Entities.RaidSummary;

/// <summary>
/// Представляет участника рейда в базе данных.
/// Связывает рейд с участником через их идентификаторы.
/// </summary>
[Table("raid_participant", Schema = DatabaseConsts.PortalSchema)]
public class RaidParticipant
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
}