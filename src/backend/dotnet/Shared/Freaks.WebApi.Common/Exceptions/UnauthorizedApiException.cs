using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions;

/// <summary>
///     Исключение, выбрасываемое при отсутствии авторизации пользователя.
///     Используется для обозначения того, что пользователь не прошёл проверку подлинности
///     или не имеет действительного контекста пользователя.
///     Наследуется от <see cref="BadRequestApiException" /> и возвращает статус <c>401 Unauthorized</c>
///     и код ошибки <c>UNAUTHORIZED</c>.
/// </summary>
public class UnauthorizedApiException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

    /// <inheritdoc />
    public override string ErrorCode => "UNAUTHORIZED";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UnauthorizedApiException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public UnauthorizedApiException(string? message = "User not authorized", Exception? innerException = null)
        : base(message, innerException)
    {
    }
}