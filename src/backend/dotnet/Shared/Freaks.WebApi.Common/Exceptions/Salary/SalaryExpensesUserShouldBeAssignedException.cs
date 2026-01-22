using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions.Salary;

/// <summary>
///     Исключение, возникающее при попытке создать целевой расход (TargetMember) без указания пользователя.
///     Для расходов типа TargetMember обязательно должен быть назначен конкретный пользователь.
/// </summary>
public class SalaryExpensesUserShouldBeAssignedException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "SALARY_EXPENSES_USER_SHOULD_BE_ASSIGNED";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryExpensesUserShouldBeAssignedException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public SalaryExpensesUserShouldBeAssignedException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}