using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions.Salary;

/// <summary>
///     Исключение, возникающее при попытке выполнить действие, требующее открытой регистрации,
///     когда регистрация на зарплатный период уже завершена.
/// </summary>
public class SalaryRegistrationEndedException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "SALARY_REGISTRATION_ENDED_EXCEPTION";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryRegistrationEndedException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public SalaryRegistrationEndedException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}