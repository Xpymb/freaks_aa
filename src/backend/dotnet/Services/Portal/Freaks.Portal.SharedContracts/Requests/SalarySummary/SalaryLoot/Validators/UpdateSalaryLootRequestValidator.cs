using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot.Validators;

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