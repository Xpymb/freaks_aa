namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

public interface ISalaryCalculationService
{
    Task CalculateAsync(long salaryId);
}