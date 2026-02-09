using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader.Validators;

/// <summary>
///     Валидатор для запроса на создание записи о доле руководства гильдии.
///     Проверяет, что LootId, Quantity и PricePerItem больше нуля.
/// </summary>
public class CreateSalaryGuildLeaderRequestValidator : AbstractValidator<CreateSalaryGuildLeaderRequest>
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="CreateSalaryGuildLeaderRequestValidator"/>.
    /// </summary>
    public CreateSalaryGuildLeaderRequestValidator()
    {
        RuleFor(x => x.SalaryLootId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}