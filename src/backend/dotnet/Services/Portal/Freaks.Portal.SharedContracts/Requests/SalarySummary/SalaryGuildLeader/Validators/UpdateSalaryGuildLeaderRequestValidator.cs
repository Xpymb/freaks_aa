using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader.Validators;

/// <summary>
///     Валидатор для запроса на обновление записи о доле руководства гильдии.
///     Проверяет, что LootId, Quantity и PricePerItem больше нуля.
/// </summary>
public class UpdateSalaryGuildLeaderRequestValidator : AbstractValidator<UpdateSalaryGuildLeaderRequest>
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UpdateSalaryGuildLeaderRequestValidator"/>.
    /// </summary>
    public UpdateSalaryGuildLeaderRequestValidator()
    {
        RuleFor(x => x.LootId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.PricePerItem).GreaterThan(0);
    }
}