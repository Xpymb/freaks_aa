using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary.Validators;

/// <summary>
///     Валидатор для запроса на обновление зарплатного периода.
///     Проверяет обязательность названия, корректность дат, наличие разрешенных типов выплат и боссов.
/// </summary>
public class UpdateSalaryRequestValidator : AbstractValidator<UpdateSalaryRequest>
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UpdateSalaryRequestValidator"/>.
    /// </summary>
    public UpdateSalaryRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.StartDt).GreaterThan(default(DateOnly));
        RuleFor(x => x.EndDt).GreaterThan(default(DateOnly));
        RuleFor(x => x.AllowedPaymentTypes).NotEmpty();
        RuleFor(x => x.BossTypes).NotEmpty();
    }
}