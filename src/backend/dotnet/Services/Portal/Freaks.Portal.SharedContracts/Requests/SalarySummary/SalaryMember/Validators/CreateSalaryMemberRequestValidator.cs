using FluentValidation;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember.Validators;

/// <summary>
///     Валидатор для запроса на создание участника зарплатного периода (для Member).
///     Проверяет корректность типа выплаты.
/// </summary>
public class CreateSalaryMemberRequestValidator : AbstractValidator<CreateSalaryMemberRequest>
{
    public CreateSalaryMemberRequestValidator()
    {
        var salaryPaymentTypes = Enum.GetValues<SalaryPaymentType>();
        RuleFor(x => x.PaymentType).Must(x => salaryPaymentTypes.Contains(x));
    }
}