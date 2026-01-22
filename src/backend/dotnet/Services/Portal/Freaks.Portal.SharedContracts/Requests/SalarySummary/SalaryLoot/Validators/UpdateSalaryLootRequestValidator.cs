using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot.Validators;

/// <summary>
///     Валидатор для запроса на обновление проданного лута в зарплатном периоде.
///     Проверяет, что LootId, Quantity и PricePerItem больше нуля,
///     а DiscountPercent и Amount не являются отрицательными значениями.
/// </summary>
public class UpdateSalaryLootRequestValidator : AbstractValidator<UpdateSalaryLootRequest>
{
    public UpdateSalaryLootRequestValidator()
    {
        RuleFor(x => x.LootId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.PricePerItem).GreaterThan(0);
        RuleFor(x => x.DiscountPercent).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
    }
}