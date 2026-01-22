using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;
using Freaks.WebApi.Common.Exceptions.Salary;

namespace Freaks.WebApi.Common.Exceptions;

/// <summary>
///     Исключение, возникающее при ошибках валидации входных данных.
///     Содержит детальную информацию об ошибках валидации для каждого поля.
/// </summary>
public class ValidationApiException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "VALIDATION";

    /// <summary>
    ///     Ошибки валидации
    /// </summary>
    public IDictionary<string, string[]> Errors { get; init; }

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ValidationApiException" />.
    /// </summary>
    /// <param name="validationProblems">Словарь ошибок валидации, где ключ - имя поля, значение - массив сообщений об ошибках.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public ValidationApiException(IDictionary<string, string[]> validationProblems, string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
        Errors = validationProblems;
    }
}