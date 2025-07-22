using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Users.Contracts;

namespace Freaks.Portal.Contracts.Entities.RaidSummary;

/// <summary>
/// Представляет сущность рейда в базе данных.
/// Содержит информацию о создателе, типе босса, формате, времени и описании рейда.
/// Также включает статус рейда, связанные скриншоты, участников и полученные трофеи.
/// Используется для планирования и отслеживания рейдовых событий в системе.
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

    /// <summary>
    ///     Текущий статус рейда (запланирован, завершён и т.д.).
    /// </summary>
    [Column("status")]
    public RaidStatus Status { get; init; } = RaidStatus.Planned;

    /// <summary>
    ///     Навигационное свойство — создатель рейда.
    /// </summary>
    public User? Creator { get; init; }

    /// <summary>
    ///     Список участников, присоединившихся к рейду.
    /// </summary>
    public IList<RaidParticipant> Participants { get; init; } = new List<RaidParticipant>();

    /// <summary>
    ///     Список скриншотов, загруженных после рейда.
    /// </summary>
    public IList<RaidScreenshot> Screenshots { get; init; } = new List<RaidScreenshot>();

    /// <summary>
    ///     Список предметов/трофеев, полученных в ходе рейда.
    /// </summary>
    public IList<RaidLoot> Loots { get; init; } = new List<RaidLoot>();
}
