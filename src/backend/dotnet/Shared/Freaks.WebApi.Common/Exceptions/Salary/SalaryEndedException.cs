using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions.Salary;

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