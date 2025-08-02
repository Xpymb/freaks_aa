using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;

namespace Freaks.Portal.Contracts.Entities.Notification;

[Table("notification_channel_message", Schema = DatabaseConsts.PortalSchema)]
public class NotificationChannelMessage : IEntity<long>
{
    /// <summary>
    /// Id сообщения
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; init; }
    
    /// <summary>
    /// Внешний ключ на NotificationChannel
    /// </summary>
    [Column("notification_channel_id")]
    public int NotificationChannelId { get; init; }

    /// <summary>
    /// Id пользователя
    /// </summary>
    [Column("creator_id")]
    public Guid CreatorId { get; init; }

    /// <summary>
    /// Содержимое сообщения
    /// </summary>
    [Column("message")]
    public required string Message { get; init; }

    /// <summary>
    /// Время создания
    /// </summary>
    [Column("created_dt")]
    public DateTimeOffset CreatedDt { get; init; }
    
    /// <summary>
    /// Навигация 
    /// </summary>
    public NotificationChannel? NotificationChannel { get; init; }
}