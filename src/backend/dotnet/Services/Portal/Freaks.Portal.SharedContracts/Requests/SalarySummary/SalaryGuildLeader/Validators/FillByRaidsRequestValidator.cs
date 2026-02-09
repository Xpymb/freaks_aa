using FluentValidation;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader.Validators;

/// <summary>
///     Валидатор для запроса <see cref="FillByRaidsRequest"/>.
/// </summary>
public class FillByRaidsRequestValidator : AbstractValidator<FillByRaidsRequest>
{
    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="FillByRaidsRequestValidator"/> и настраивает правила валидации.
    /// </summary>
    public FillByRaidsRequestValidator()
    {
        RuleFor(x => x.LootIds).NotEmpty();
    }
}