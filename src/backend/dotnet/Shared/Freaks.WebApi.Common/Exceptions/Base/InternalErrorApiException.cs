using System.Net;

namespace Freaks.WebApi.Common.Exceptions.Base;

/// <summary>
///     Представляет исключение, связанное с внутренней ошибкой сервера (500 Internal Server Error).
///     Используется для сигнализации о непредвиденных ошибках в работе сервера.
/// </summary>
public class InternalErrorApiException : BaseApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    /// <inheritdoc />
    public override string ErrorCode => "INTERNAL_SERVER_EXCEPTION";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="InternalErrorApiException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public InternalErrorApiException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}