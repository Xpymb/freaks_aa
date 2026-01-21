using System.Net;

namespace Freaks.WebApi.Common.Exceptions.Base;

/// <summary>
///     Базовое исключение для API, содержащее статус HTTP и внутренний код ошибки.
/// </summary>
public abstract class BaseApiException : Exception
{
    /// <summary>
    ///     HTTP-статус ответа, связанный с исключением.
    /// </summary>
    public virtual HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    /// <summary>
    ///     Внутренний код ошибки, используемый для идентификации типа ошибки на клиенте.
    /// </summary>
    public virtual string ErrorCode => "BASE_API_EXCEPTION";

    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="BaseApiException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    protected BaseApiException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {
    }
}