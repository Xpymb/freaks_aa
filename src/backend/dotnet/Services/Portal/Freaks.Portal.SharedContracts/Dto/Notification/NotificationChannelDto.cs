namespace Freaks.Portal.SharedContracts.Dto.Notification;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="ChannelId"></param>
/// <param name="Name"></param>
public record NotificationChannelDto(
    int Id,
    long ChannelId,
    string Name);