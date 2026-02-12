using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions.Salary;

/// <summary>
///     Исключение, выбрасываемое при попытке выполнить действие до завершения регистрации на зарплатный период.
/// </summary>
public class SalaryRegistrationNotEndedYetException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "SALARY_REGISTRATION_NOT_ENDED_YET_EXCEPTION";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryRegistrationNotEndedYetException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public SalaryRegistrationNotEndedYetException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}