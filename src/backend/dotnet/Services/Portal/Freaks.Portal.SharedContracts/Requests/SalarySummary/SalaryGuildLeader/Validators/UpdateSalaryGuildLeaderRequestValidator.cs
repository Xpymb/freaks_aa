using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader.Validators;

public class UpdateSalaryGuildLeaderRequestValidator : AbstractValidator<UpdateSalaryGuildLeaderRequest>
{
    public UpdateSalaryGuildLeaderRequestValidator()
    {
        RuleFor(x => x.LootId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.PricePerItem).GreaterThan(0);
    }
}