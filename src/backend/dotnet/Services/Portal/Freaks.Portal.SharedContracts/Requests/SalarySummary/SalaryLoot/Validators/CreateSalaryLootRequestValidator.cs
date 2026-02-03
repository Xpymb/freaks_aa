using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot.Validators;

/// <summary>
///     Валидатор для запроса на добавление проданного лута в зарплатный период.
///     Проверяет, что LootId, Quantity и PricePerItem больше нуля,
///     и обязательность заполнения хотя бы одного из полей: DiscountPercent или Amount.
/// </summary>
public class CreateSalaryLootRequestValidator : AbstractValidator<CreateSalaryLootRequest>
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="CreateSalaryLootRequestValidator"/>.
    /// </summary>
    public CreateSalaryLootRequestValidator()
    {
        RuleFor(x => x.LootId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.PricePerItem).GreaterThan(0);

        RuleFor(x => x).Must(x => x.DiscountPercent.HasValue || x.Amount.HasValue);

        RuleFor(x => x.DiscountPercent).GreaterThanOrEqualTo(0).When(x => x.DiscountPercent.HasValue);
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).When(x => x.Amount.HasValue);
    }
}