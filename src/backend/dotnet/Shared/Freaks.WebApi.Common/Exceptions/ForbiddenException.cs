using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions;

/// <summary>
///     Исключение, выбрасываемое при попытке выполнить действие без достаточных прав (Forbidden).
/// </summary>
public class ForbiddenException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

    /// <inheritdoc />
    public override string ErrorCode => "FORBIDDEN";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ForbiddenException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public ForbiddenException(string? message = "Forbidden", Exception? innerException = null)
        : base(message, innerException)
    {
    }
}