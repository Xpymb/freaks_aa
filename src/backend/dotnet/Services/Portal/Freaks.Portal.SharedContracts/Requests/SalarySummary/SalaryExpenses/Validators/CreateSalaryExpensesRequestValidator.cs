using FluentValidation;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryExpenses.Validators;

public class CreateSalaryExpensesRequestValidator : AbstractValidator<CreateSalaryExpensesRequest>
{
    public CreateSalaryExpensesRequestValidator()
    {
        var salaryExpenseTypes = Enum.GetValues<SalaryExpensesType>();
        RuleFor(x => x.ExpensesType).Must(x => salaryExpenseTypes.Contains(x));

        When(x => x.ExpensesType is SalaryExpensesType.TargetMember, () => RuleFor(x => x.UserId).NotNull().NotEmpty());

        RuleFor(x => x)
            .Must(x => x.Percentage.HasValue || x.Amount.HasValue)
            .WithMessage("Должно быть заполнено хотя бы одно из полей: Percentage или Amount");

        RuleFor(x => x.Percentage)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Percentage.HasValue);

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Amount.HasValue);
    }
}