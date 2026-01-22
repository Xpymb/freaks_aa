using FluentValidation;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember.Validators;

/// <summary>
///     Валидатор для запроса на создание участника зарплатного периода (для Admin/Editor/GuildLeader).
///     Проверяет обязательность UserId и корректность типа выплаты.
/// </summary>
public class CreateSalaryMemberAdminRequestValidator : AbstractValidator<CreateSalaryMemberAdminRequest>
{
    public CreateSalaryMemberAdminRequestValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty();

        var salaryPaymentTypes = Enum.GetValues<SalaryPaymentType>();
        RuleFor(x => x.PaymentType).Must(x => salaryPaymentTypes.Contains(x));
    }
}