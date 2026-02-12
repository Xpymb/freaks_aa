using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Bll.Implementation.SalarySummary.Helpers;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.ValueObjects.Loot;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
using Freaks.WebApi.Common.Exceptions;
using Freaks.WebApi.Common.Exceptions.Salary;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

/// <summary>
///     Сервис для работы с итоговым отчётом зарплатного периода.
///     Формирует и рассчитывает итоговые данные по распределению лута, расходам и выплатам.
/// </summary>
public class SalaryFinalReportService : ISalaryFinalReportService
{
    private readonly IMapper _mapper;
    private readonly ISalaryProvider _salaryProvider;
    private readonly ISalaryLootProvider _lootProvider;
    private readonly ISalaryGuildLeaderProvider _guildLeaderProvider;
    private readonly ISalaryExpensesProvider _expensesProvider;
    private readonly ISalaryFinalReportProvider _provider;
    private readonly ISalaryStepService _stepService;
    private readonly ISalaryCalculationService _calculationService;
    private readonly IUnitOfWork<PortalDbContext> _uow;

    /// <summary>
    ///     Инициализирует новый экземпляр сервиса <see cref="SalaryFinalReportService" />.
    /// </summary>
    /// <param name="mapper">Сервис Mapster для преобразования между моделями.</param>
    /// <param name="provider">Провайдер для работы с итоговыми отчётами.</param>
    /// <param name="salaryProvider">Провайдер для работы с зарплатными периодами.</param>
    /// <param name="lootProvider">Провайдер для работы с проданным лутом.</param>
    /// <param name="guildLeaderProvider">Провайдер для работы с долями руководства.</param>
    /// <param name="expensesProvider">Провайдер для работы с расходами.</param>
    /// <param name="stepService">Сервис степпера зарплатных периодов.</param>
    /// <param name="calculationService">Сервис расчёта зарплат.</param>
    /// <param name="uow">Unit of work.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из аргументов равен null.</exception>
    public SalaryFinalReportService(
        IMapper mapper,
        ISalaryFinalReportProvider provider,
        ISalaryProvider salaryProvider,
        ISalaryLootProvider lootProvider,
        ISalaryGuildLeaderProvider guildLeaderProvider,
        ISalaryExpensesProvider expensesProvider,
        ISalaryStepService stepService,
        ISalaryCalculationService calculationService,
        IUnitOfWork<PortalDbContext> uow)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _salaryProvider = salaryProvider ?? throw new ArgumentNullException(nameof(salaryProvider));
        _lootProvider = lootProvider ?? throw new ArgumentNullException(nameof(lootProvider));
        _guildLeaderProvider = guildLeaderProvider ?? throw new ArgumentNullException(nameof(guildLeaderProvider));
        _expensesProvider = expensesProvider ?? throw new ArgumentNullException(nameof(expensesProvider));
        _stepService = stepService ?? throw new ArgumentNullException(nameof(stepService));
        _calculationService = calculationService ?? throw new ArgumentNullException(nameof(calculationService));
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
    }

    /// <inheritdoc />
    public Task<SalaryFinalReportDto> GetAsync(long salaryId)
    {
        return _uow.ExecuteInsideTransactionAsync(async _ =>
        {
            var salary = await _salaryProvider.GetAsync(salaryId, EntityTrackingType.NoTracking);
            if (salary is null)
            {
                throw new EntityNotFoundException(nameof(Salary));
            }

            if (salary.RegistrationStatus is not SalaryRegistrationStatus.Ended)
            {
                throw new SalaryRegistrationNotEndedYetException();
            }

            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.FinalReports);

            var finalReport = await _provider.GetAsync(salaryId, EntityTrackingType.NoTracking);
            if (finalReport is null)
            {
                finalReport = new SalaryFinalReport
                {
                    Id = salaryId,
                };
                await _provider.CreateAsync(finalReport);

                await _calculationService.CalculateAsync(salaryId);
            }

            if (salary.IsFinished)
            {
                return _mapper.Map<SalaryFinalReportDto>(finalReport);
            }

            var salaryLoot = await _lootProvider.GetBySalaryIdAsync(salaryId);
            var guildLeaderExpenses = await _guildLeaderProvider.GetBySalaryIdAsync(salaryId);
            var expenses = await _expensesProvider.GetBySalaryIdAsync(salaryId);

            // Общий итог зарплатного периода
            finalReport.TotalGold = salaryLoot
                .Where(l => l.LootItem!.Type is not LootType.WorldBossInfusion and LootType.ErenorInfusion)
                .Sum(x => x.Amount);
            finalReport.TotalWorldBossInfusion = salaryLoot
                .Where(l => l.LootItem!.Type is LootType.WorldBossInfusion)
                .Sum(x => x.Quantity * x.LootItem!.SynthesisExp!.Value);
            finalReport.TotalWorldBossInfusion = salaryLoot
                .Where(l => l.LootItem!.Type is LootType.ErenorInfusion)
                .Sum(x => x.Quantity * x.LootItem!.SynthesisExp!.Value);

            // Итого сколько забрал ГЛ
            finalReport.GoldGuildLeaderExpenses = guildLeaderExpenses
                .Where(gl => gl.SalaryLoot!.LootItem!.Type is not LootType.WorldBossInfusion and LootType.ErenorInfusion)
                .Sum(gl => SalaryGuildLeaderHelper.CalculateAmount(gl.SalaryLoot, gl.Quantity));
            finalReport.WorldBossInfusionGuildLeaderExpenses = guildLeaderExpenses
                .Where(gl => gl.SalaryLoot!.LootItem!.Type is LootType.WorldBossInfusion)
                .Sum(gl => gl.SalaryLoot!.Quantity * gl.SalaryLoot!.LootItem!.SynthesisExp!.Value);
            finalReport.ErenorInfusionGuildLeaderExpenses = guildLeaderExpenses
                .Where(gl => gl.SalaryLoot!.LootItem!.Type is LootType.ErenorInfusion)
                .Sum(gl => gl.SalaryLoot!.Quantity * gl.SalaryLoot!.LootItem!.SynthesisExp!.Value);

            // Итого сколько ушло на расходы гильдии
            finalReport.GoldExpenses = expenses.Sum(x => x.Amount);
            finalReport.WorldBossInfusionExpenses = 0; // TODO
            finalReport.ErenorInfusionExpenses = 0; // TODO

            await _provider.UpdateAsync(finalReport);

            return _mapper.Map<SalaryFinalReportDto>(finalReport);
        });
    }
}