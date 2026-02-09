using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryExpenses.Validators;

/// <summary>
///     Валидатор для запроса на обновление записи о расходах гильдии.
///     Проверяет, что Percentage и Amount не являются отрицательными значениями.
/// </summary>
public class UpdateSalaryExpensesRequestValidator : AbstractValidator<UpdateSalaryExpensesRequest>
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UpdateSalaryExpensesRequestValidator"/>.
    /// </summary>
    public UpdateSalaryExpensesRequestValidator()
    {
        RuleFor(x => x.Percentage)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0);
    }
}