using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary.Validators;

/// <summary>
///     Валидатор для запроса на создание нового зарплатного периода.
///     Проверяет обязательность названия, корректность дат, наличие разрешенных типов выплат и боссов.
/// </summary>
public class CreateSalaryRequestValidator : AbstractValidator<CreateSalaryRequest>
{
    public CreateSalaryRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.StartDt).GreaterThan(default(DateOnly));
        RuleFor(x => x.EndDt).GreaterThan(default(DateOnly));
        RuleFor(x => x.AllowedPaymentTypes).NotEmpty();
        RuleFor(x => x.BossTypes).NotEmpty();
    }
}