using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
using Freaks.WebApi.Common.Exceptions;
using Freaks.WebApi.Common.Exceptions.Salary;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

public class SalaryStepService : ISalaryStepService
{
    private readonly ISalaryProvider _salaryProvider;

    public SalaryStepService(ISalaryProvider salaryProvider)
    {
        _salaryProvider = salaryProvider ?? throw new ArgumentNullException(nameof(salaryProvider));
    }

    /// <inheritdoc />
    public async Task HandleStepActionAsync(long salaryId, SalaryFillStepType fillStepType)
    {
        var salary = await _salaryProvider.GetAsync(salaryId, EntityTrackingType.NoTracking);
        if (salary is null)
        {
            throw new EntityNotFoundException(nameof(Salary));
        }

        CheckEnded(salary);

        if(salary.FillStepType < fillStepType)
        {
            salary.FillStepType = fillStepType;
        }

        await _salaryProvider.UpdateAsync(salary);
    }

    /// <inheritdoc />
    public async Task CheckAccessAsync(long salaryId, SalaryActionType actionType)
    {
        var salary = await _salaryProvider.GetAsync(salaryId, EntityTrackingType.NoTracking);
        if (salary is null)
        {
            throw new EntityNotFoundException(nameof(Salary));
        }

        CheckEnded(salary);
    }

    private static void CheckEnded(Salary salary)
    {
        if (salary.FillStepType is SalaryFillStepType.FinalReports &&
            salary.RegistrationStatus is SalaryRegistrationStatus.Ended)
        {
            throw new SalaryEndedException();
        }
    }

    private static void CheckRegistrationStatus(Salary salary)
    {
        if (salary.RegistrationStatus is SalaryRegistrationStatus.Ended)
        {
            throw new SalaryRegistrationEndedException();
        }
    }
}