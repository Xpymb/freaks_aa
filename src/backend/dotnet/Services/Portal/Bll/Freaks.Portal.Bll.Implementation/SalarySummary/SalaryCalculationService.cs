using Freaks.Common.Extensions;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Bll.Implementation.SalarySummary.Algorithms;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.WebApi.Common.Exceptions;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

/// <summary>
///     Реализация сервиса для расчета зарплат участников за зарплатный период.
///     Выполняет расчет на основе рейдов, лута, расходов и участников с использованием алгоритмов распределения.
/// </summary>
public class SalaryCalculationService : ISalaryCalculationService
{
    private readonly ISalaryProvider _salaryProvider;
    private readonly ISalaryLootProvider _salaryLootProvider;
    private readonly ISalaryGuildLeaderProvider _salaryGuildLeaderProvider;
    private readonly ISalaryExpensesProvider _salaryExpensesProvider;
    private readonly ISalaryMemberProvider _salaryMemberProvider;
    private readonly IRaidProvider _raidProvider;

    /// <summary>
    ///     Инициализирует новый экземпляр сервиса <see cref="SalaryCalculationService" />.
    /// </summary>
    /// <param name="salaryProvider">Провайдер для работы с зарплатными периодами.</param>
    /// <param name="salaryLootProvider">Провайдер для работы с проданным лутом.</param>
    /// <param name="salaryGuildLeaderProvider">Провайдер для работы с долями руководства.</param>
    /// <param name="salaryExpensesProvider">Провайдер для работы с расходами.</param>
    /// <param name="salaryMemberProvider">Провайдер для работы с участниками.</param>
    /// <param name="raidProvider">Провайдер для работы с рейдами.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из аргументов равен null.</exception>
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