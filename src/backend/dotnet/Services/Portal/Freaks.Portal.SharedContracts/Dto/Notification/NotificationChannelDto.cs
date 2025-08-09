namespace Freaks.Portal.SharedContracts.Dto.Notification;

/// <summary>
/// DTO для передачи информации о Discord-канале.
/// Используется для безопасного представления данных канала клиентским приложениям.
/// </summary>
/// <param name="Id">Внутренний идентификатор канала в системе</param>
/// <param name="ChannelId">Идентификатор канала в Discord </param>
/// <param name="Name">Название канала, полученное из Discord</param>
public record NotificationChannelDto(
    int Id,
    long ChannelId,
    string Name);