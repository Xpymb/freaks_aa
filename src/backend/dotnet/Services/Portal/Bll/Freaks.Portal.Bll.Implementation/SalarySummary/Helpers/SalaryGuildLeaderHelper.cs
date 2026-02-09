using Freaks.Portal.Contracts.Entities.SalarySummary;

namespace Freaks.Portal.Bll.Implementation.SalarySummary.Helpers;

public static class SalaryGuildLeaderHelper
{
    public static decimal CalculateAmount(SalaryLoot? salaryLoot, int quantity)
    {
        var result = salaryLoot?.Amount / salaryLoot?.Quantity * quantity;

        return result.GetValueOrDefault(0);
    }
}