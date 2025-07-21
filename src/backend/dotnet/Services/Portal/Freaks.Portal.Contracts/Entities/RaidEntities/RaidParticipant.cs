using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Dal.Common.Consts;
using Freaks.Users.Contracts;

namespace Freaks.Portal.Contracts.Entities.RaidEntities;

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
    
    public Raid? Raid { get; init; }
    
    public User? User { get; init; }
}