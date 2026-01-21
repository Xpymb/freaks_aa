using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

public interface ISalaryStepService
{
    Task HandleStepActionAsync(long salaryId, SalaryFillStepType fillStepType);

    Task CheckAccessAsync(long salaryId, SalaryActionType actionType);
}