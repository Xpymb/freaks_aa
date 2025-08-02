using Freaks.Dal.Common.ValueObjects;

namespace Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
/// <summary>
/// 
/// </summary>
/// <param name="sortBy"></param>
/// <param name="SortMode"></param>
/// <param name="Take"></param>
/// <param name="Skip"></param>
public record GetNotificationChannelMessageRequest(
    NotificationChannelMessageSortBy sortBy,
    OrderByMode SortMode,
    int? Take,
    int? Skip);