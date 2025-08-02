namespace Freaks.Portal.SharedContracts.Dto.Notification;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="CreatorId"></param>
/// <param name="Name"></param>
/// <param name="CreatedDt"></param>
public record NotificationChannelMessageDto(
    long Id,
    Guid CreatorId,
    string Name,
    DateTimeOffset CreatedDt);