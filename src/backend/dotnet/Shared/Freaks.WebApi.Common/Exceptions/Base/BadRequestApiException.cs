using System.Net;

namespace Freaks.WebApi.Common.Exceptions.Base;

/// <summary>
///     Представляет исключение, связанное с ошибкой 400 (Bad Request).
///     Используется для сигнализации о неверных данных или запросах клиента.
/// </summary>
public class BadRequestApiException : BaseApiException
{
    /// <summary>
    ///     HTTP-статус, возвращаемый при данной ошибке (400 Bad Request).
    /// </summary>
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <summary>
    ///     Внутренний код ошибки для клиента или логирования.
    /// </summary>
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