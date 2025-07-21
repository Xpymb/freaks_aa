using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions;

/// <summary>
///     Исключение, указывающее, что запрашиваемая сущность не найдена.
///     Возвращает статус 404 Not Found и используется, когда клиент обращается к несуществующим данным.
/// </summary>
public class EntityNotFoundException : BadRequestApiException
{
    /// <summary>
    ///     HTTP-статус, возвращаемый при данной ошибке (404 Not Found).
    /// </summary>
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    /// <summary>
    ///     Внутренний код ошибки для логирования или обработки на клиенте.
    /// </summary>
    public override string ErrorCode => "ENTITY_NOT_FOUND";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="EntityNotFoundException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public EntityNotFoundException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}