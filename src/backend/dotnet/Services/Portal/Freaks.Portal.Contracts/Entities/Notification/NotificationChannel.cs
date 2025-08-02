using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;

namespace Freaks.Portal.Contracts.Entities.Notification;

[Table("notification_channel", Schema = DatabaseConsts.PortalSchema)]
public class NotificationChannel : IEntity<int>
{
    /// <summary>
    /// Id канала
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; init; }

    /// <summary>
    /// Discord Id
    /// </summary>
    [Column("discord_channel_id")]
    public required long DiscordChannelId { get; init; }

    /// <summary>
    /// Название канала
    /// </summary>
    [Column("name")]
    public required string Name { get; init; } 
    
    /// <summary>
    /// Сообщения в этом канале
    /// </summary>
    public IList<NotificationChannelMessage> Messages { get; init; } = new List<NotificationChannelMessage>();
}