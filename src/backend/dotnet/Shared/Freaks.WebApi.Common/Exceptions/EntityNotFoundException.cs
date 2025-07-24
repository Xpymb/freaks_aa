using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions;

/// <summary>
///     Исключение, указывающее, что запрашиваемая сущность не найдена.
///     Возвращает статус 404 Not Found и используется, когда клиент обращается к несуществующим данным.
/// </summary>
public class EntityNotFoundException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    /// <inheritdoc />
    public override string ErrorCode => "ENTITY_NOT_FOUND";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="EntityNotFoundException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public EntityNotFoundException(string? message = "Entity not found", Exception? innerException = null)
        : base(message, innerException)
    {
    }
}