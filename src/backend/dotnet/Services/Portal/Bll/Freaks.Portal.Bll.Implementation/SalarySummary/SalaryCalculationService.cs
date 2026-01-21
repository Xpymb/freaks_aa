using Freaks.Bll.Common.Helpers;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Bll.Implementation.SalarySummary.Algorithms;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.WebApi.Common.Exceptions;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

public class SalaryCalculationService : ISalaryCalculationService
{
    private readonly ISalaryProvider _salaryProvider;
    private readonly ISalaryLootProvider _salaryLootProvider;
    private readonly ISalaryGuildLeaderProvider _salaryGuildLeaderProvider;
    private readonly ISalaryExpensesProvider _salaryExpensesProvider;
    private readonly ISalaryMemberProvider _salaryMemberProvider;
    private readonly IRaidProvider _raidProvider;

    public SalaryCalculationService(
        ISalaryProvider salaryProvider,
        ISalaryLootProvider salaryLootProvider,
        ISalaryGuildLeaderProvider salaryGuildLeaderProvider,
        ISalaryExpensesProvider salaryExpensesProvider,
        ISalaryMemberProvider salaryMemberProvider,
        IRaidProvider raidProvider)
    {
        _salaryProvider = salaryProvider ?? throw new ArgumentNullException(nameof(salaryProvider));
        _salaryLootProvider = salaryLootProvider ?? throw new ArgumentNullException(nameof(salaryLootProvider));
        _salaryGuildLeaderProvider = salaryGuildLeaderProvider ?? throw new ArgumentNullException(nameof(salaryGuildLeaderProvider));
        _salaryExpensesProvider = salaryExpensesProvider ?? throw new ArgumentNullException(nameof(salaryExpensesProvider));
        _salaryMemberProvider = salaryMemberProvider ?? throw new ArgumentNullException(nameof(salaryMemberProvider));
        _raidProvider = raidProvider ?? throw new ArgumentNullException(nameof(raidProvider));
    }

    /// <inheritdoc />
    public async Task CalculateAsync(long salaryId)
    {
        var salary = await _salaryProvider.GetAsync(salaryId, EntityTrackingType.NoTracking);
        if (salary is null)
        {
            throw new EntityNotFoundException(nameof(Salary));
        }

        var salaryLoot = await _salaryLootProvider.GetBySalaryIdAsync(salaryId);
        var salaryGuildLeader = await _salaryGuildLeaderProvider.GetBySalaryIdAsync(salaryId);
        var salaryExpenses = await _salaryExpensesProvider.GetBySalaryIdAsync(salaryId);
        var salaryMembers = await _salaryMemberProvider.GetBySalaryIdAsync(salaryId);

        var raids = await _raidProvider.GetFullInfoAsync(salary.StartDt.ToDateTimeOffset(), salary.EndDt.AddDays(1).ToDateTimeOffset(), salary.BossTypes);

        var algorithm =
            new BasicSalaryAlgorithm(raids, salary, salaryLoot, salaryGuildLeader, salaryExpenses, salaryMembers);

        var membersResult = algorithm.Calculate();
        await _salaryMemberProvider.UpdateSetAsync(membersResult);
    }
}