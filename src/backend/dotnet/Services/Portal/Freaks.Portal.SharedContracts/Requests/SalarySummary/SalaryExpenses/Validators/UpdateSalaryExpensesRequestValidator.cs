using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryExpenses.Validators;

public class UpdateSalaryExpensesRequestValidator : AbstractValidator<UpdateSalaryExpensesRequest>
{
    public UpdateSalaryExpensesRequestValidator()
    {
        RuleFor(x => x.Percentage)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0);
    }
}