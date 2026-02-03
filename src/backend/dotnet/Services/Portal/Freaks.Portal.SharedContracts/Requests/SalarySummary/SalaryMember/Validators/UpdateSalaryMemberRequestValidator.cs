using FluentValidation;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember.Validators;

/// <summary>
///     Валидатор для запроса на обновление информации об участнике зарплатного периода.
///     Проверяет корректность типа выплаты.
/// </summary>
public class UpdateSalaryMemberRequestValidator : AbstractValidator<UpdateSalaryMemberRequest>
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UpdateSalaryMemberRequestValidator"/>.
    /// </summary>
    public UpdateSalaryMemberRequestValidator()
    {
        var salaryPaymentTypes = Enum.GetValues<SalaryPaymentType>();
        RuleFor(x => x.PaymentType).Must(x => salaryPaymentTypes.Contains(x));
    }
}