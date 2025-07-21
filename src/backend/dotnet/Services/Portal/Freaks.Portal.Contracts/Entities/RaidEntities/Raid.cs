using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.SharedContracts.ValueObjects.Raid;
using Freaks.Users.Contracts;

namespace Freaks.Portal.Contracts.Entities.RaidEntities;

/// <summary>
/// Представляет сущность рейда в базе данных.
/// Содержит информацию о создателе, типе босса, формате, времени и описании рейда.
/// </summary>
[Table("raid", Schema = DatabaseConsts.PortalSchema)]
public class Raid : IEntity<int>
{
    /// <summary>
    /// Уникальный идентификатор рейда (первичный ключ).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; init; }

    /// <summary>
    /// Идентификатор пользователя, создавшего рейд.
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    /// Тип босса, против которого будет проходить рейд.
    /// </summary>
    [Column("boss_type")]
    public required BossType BossType { get; init; }

    /// <summary>
    /// Формат рейда, может быть null.
    /// </summary>
    [Column("format_type")]
    public RaidFormatType? FormatType { get; set; }

    /// <summary>
    /// Дата и время начала рейда.
    /// </summary>
    [Column("start_dt")]
    public required DateTimeOffset StartDt { get; init; }

    /// <summary>
    /// Дата и время создания записи о рейде.
    /// </summary>
    [Column("created_dt")]
    public required DateTimeOffset CreatedDt { get; init; }

    /// <summary>
    /// Дата и время последнего обновления записи, может быть null.
    /// </summary>
    [Column("updated_dt")]
    public DateTimeOffset? UpdatedDt { get; set; }

    /// <summary>
    /// Описание рейда: дополнительные заметки.
    /// </summary>
    [Column("description")]
    public required string Description { get; init; }
    
    public User? Creator { get; init; }
    
    public IList<RaidParticipant> Participants { get; init; } = new List<RaidParticipant>();
    public IList<RaidScreenshot> Screenshots { get; init; } = new List<RaidScreenshot>();
    public IList<RaidLoot> Loots { get; init; } = new List<RaidLoot>();
}