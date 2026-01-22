using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions.Salary;

/// <summary>
///     Исключение, возникающее при попытке изменить завершенный зарплатный период.
///     После завершения зарплатного периода изменения данных не допускаются.
/// </summary>
public class SalaryEndedException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "SALARY_ENDED";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryEndedException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public SalaryEndedException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}