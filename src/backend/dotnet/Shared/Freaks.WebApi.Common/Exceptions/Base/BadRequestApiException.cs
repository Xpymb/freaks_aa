using System.Net;

namespace Freaks.WebApi.Common.Exceptions.Base;

/// <summary>
///     Представляет исключение, связанное с ошибкой 400 (Bad Request).
///     Используется для сигнализации о неверных данных или запросах клиента.
/// </summary>
public class BadRequestApiException : BaseApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "BAD_REQUEST";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="BadRequestApiException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public BadRequestApiException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}