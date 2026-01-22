using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot.Validators;

public class CreateSalaryLootRequestValidator : AbstractValidator<CreateSalaryLootRequest>
{
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