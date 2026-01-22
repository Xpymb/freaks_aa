using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary.Validators;

public class UpdateSalaryRequestValidator : AbstractValidator<UpdateSalaryRequest>
{
    public UpdateSalaryRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.StartDt).GreaterThan(default(DateOnly));
        RuleFor(x => x.EndDt).GreaterThan(default(DateOnly));
        RuleFor(x => x.AllowedPaymentTypes).NotEmpty();
        RuleFor(x => x.BossTypes).NotEmpty();
    }
}