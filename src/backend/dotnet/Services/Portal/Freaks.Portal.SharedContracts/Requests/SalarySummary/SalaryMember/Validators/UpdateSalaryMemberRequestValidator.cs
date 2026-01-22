using FluentValidation;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember.Validators;

public class UpdateSalaryMemberRequestValidator : AbstractValidator<UpdateSalaryMemberRequest>
{
    public UpdateSalaryMemberRequestValidator()
    {
        var salaryPaymentTypes = Enum.GetValues<SalaryPaymentType>();
        RuleFor(x => x.PaymentType).Must(x => salaryPaymentTypes.Contains(x));
    }
}