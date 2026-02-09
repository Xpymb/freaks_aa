using FluentValidation;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary.Validators;

/// <summary>
///     Валидатор для запроса на создание нового зарплатного периода.
///     Проверяет обязательность названия, корректность дат, наличие разрешенных типов выплат и боссов.
/// </summary>
public class CreateSalaryRequestValidator : AbstractValidator<CreateSalaryRequest>
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="CreateSalaryRequestValidator"/>.
    /// </summary>
    public CreateSalaryRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.StartDt).GreaterThan(default(DateOnly));
        RuleFor(x => x.EndDt).GreaterThan(default(DateOnly));

        var salaryPaymentTypes = Enum.GetValues<SalaryPaymentType>();
        RuleFor(x => x.AllowedPaymentTypes)
            .ForEach(r => r.Must(s => salaryPaymentTypes.Contains(s)));

        var bossTypes = Enum.GetValues<BossType>();
        RuleFor(x => x.BossTypes)
            .ForEach(r => r.Must(b => bossTypes.Contains(b)));
    }
}