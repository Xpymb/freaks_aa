using Freaks.Dal.Common.ValueObjects;

namespace Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;

/// <summary>
/// Запрос на получение сообщений Discord-канала с пагинацией и сортировкой.
/// </summary>
/// <param name="SortBy">Поле для сортировки сообщений</param>
/// <param name="SortMode">Направление сортировки (по возрастанию/убыванию)</param>
/// <param name="Take">Количество возвращаемых сообщений </param>
/// <param name="Skip">Количество пропускаемых сообщений</param>
public record GetNotificationChannelMessageRequest(
    NotificationChannelMessageSortBy SortBy,
    OrderByMode SortMode,
    int? Take,
    int? Skip);