namespace Freaks.Portal.SharedContracts.Dto.Notification;

/// <summary>
/// DTO для передачи информации о сообщении Discord-канала.
/// Содержит основные данные сообщения для отображения на клиенте.
/// </summary>
/// <param name="Id">Внутренний идентификатор сообщения в системе</param>
/// <param name="CreatorId">Идентификатор автора сообщения в системе</param>
/// <param name="Name">Текст сообщения</param>
/// <param name="CreatedDt">Дата и время создания сообщения </param>
public record NotificationChannelMessageDto(
    long Id,
    Guid CreatorId,
    string Name,
    DateTimeOffset CreatedDt);