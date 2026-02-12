using Freaks.Portal.Bll.Implementation.SalarySummary.Helpers;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Contracts.ValueObjects.RaidSummary;
using Freaks.Portal.Contracts.ValueObjects.SalarySummary;
using Freaks.Portal.SharedContracts.ValueObjects.Loot;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.Bll.Implementation.SalarySummary.Algorithms;

/// <summary>
///     Базовый алгоритм расчета зарплат участников гильдии.
///     Распределяет доход от проданного лута между участниками с учетом их активности, ответственности,
///     расходов и долей руководства гильдии.
/// </summary>
public class BasicSalaryAlgorithm
{
    private Salary _salary;
    private IList<SalaryLoot> _loot;
    private IList<SalaryGuildLeader> _guildLeader;
    private IList<SalaryExpenses> _expenses;
    private IList<SalaryMember> _members;
    private IList<RaidFullInfo> _raids;

    private Dictionary<Guid, MemberCalculation> _calculations = new();
    private decimal _undistributed;

    /// <summary>
    ///     Инициализирует новый экземпляр алгоритма расчета зарплат.
    /// </summary>
    public BasicSalaryAlgorithm(SalaryCalculationData calculationData)
    {
        _salary = calculationData.Salary;
        _loot = calculationData.SalaryLoots;
        _guildLeader = calculationData.SalaryGuildLeaderExpanses;
        _expenses = calculationData.SalaryExpenses;
        _members = calculationData.SalaryMembers;
        _raids = calculationData.RaidFullInfos;
    }

    /// <summary>
    ///     Главный метод расчёта зарплат
    /// </summary>
    public SalaryCalculationResults Calculate()
    {
        var emptyResult = new SalaryCalculationResults(
            new List<SalaryMemberCalculationResult>(),
            0,
            0,
            0,
            0,
            0);

        if (_members.Count == 0
            || _raids.Count == 0)
        {
            return emptyResult;
        }

        // Пул для распределения
        var distributionPool = CalculateDistributionPool();
        if (distributionPool <= 0)
        {
            return emptyResult;
        }

        // Создаём инвентарь лута для списания
        var lootInventory = BuildLootInventory();

        // Стоимость рейдов (со списанием из инвентаря)
        var raidCalcs = CalculateRaidValues(lootInventory);

        // Распределение по рейдам
        DistributeByRaids(raidCalcs, distributionPool);

        // Распределение голды за ответственность
        DistributeResponsibilityGold();

        // Сборка результатов по участникам
        var (availableWb, pricePerWb) = GetWorldBossInfusionInfo();
        var memberResults = BuildMemberResults(pricePerWb);

        // Балансировка РБ
        memberResults = BalanceWorldBossInfusions(memberResults, availableWb, pricePerWb);

        // Подсчёт итоговых значений для отчёта
        var goldForSalary = memberResults.Sum(m => m.AmountGold);
        var wbForSalary = memberResults.Sum(m => m.AmountWorldBossInfusion);
        var wbForSale = availableWb - wbForSalary;
        var erenorForSalary = memberResults.Sum(m => m.AmountErenorInfusion);

        return new SalaryCalculationResults(
            memberResults,
            goldForSalary,
            wbForSalary,
            wbForSale,
            erenorForSalary,
            0m);
    }

    /// <summary>
    ///     Шаг 1: Расчёт пула для распределения
    ///     distributionPool = totalLootAmount - guildLeaderAmount - expensesAmount
    /// </summary>
    private decimal CalculateDistributionPool()
    {
        var totalLootAmount = _loot.Sum(l => l.Amount);
        var guildLeaderAmount = _guildLeader.Sum(g => SalaryGuildLeaderHelper.CalculateAmount(g.SalaryLoot, g.Quantity));
        var expensesAmount = _expenses.Sum(e => e.Amount);

        return totalLootAmount - guildLeaderAmount - expensesAmount;
    }

    /// <summary>
    ///     Шаг 2: Создание инвентаря лута для списания
    ///     Каждая запись SalaryLoot становится элементом инвентаря с остатком quantity
    ///     EffectivePrice = PricePerItem * (1 - DiscountPercent / 100)
    /// </summary>
    private List<LootInventoryItem> BuildLootInventory()
    {
        return _loot
            .Select(l => new LootInventoryItem
            {
                LootId = l.LootId,
                EffectivePrice = l.PricePerItem * (1 - l.DiscountPercent / 100m),
                RemainingQuantity = l.Quantity
            })
            .ToList();
    }

    /// <summary>
    ///     Шаг 3: Расчёт стоимости каждого рейда со списанием из инвентаря
    ///     Гарантирует, что сумма стоимостей рейдов = totalLootAmount
    /// </summary>
    private List<RaidCalculation> CalculateRaidValues(List<LootInventoryItem> inventory)
    {
        var result = new List<RaidCalculation>();

        foreach (var raidInfo in _raids)
        {
            var raidValue = 0m;

            foreach (var raidLoot in raidInfo.Loot)
            {
                var needQuantity = raidLoot.Quantity;

                // Списываем из инвентаря пока не наберём нужное количество
                foreach (var invItem in inventory.Where(i => i.LootId == raidLoot.LootItemId && i.RemainingQuantity > 0))
                {
                    if (needQuantity <= 0)
                    {
                        break;
                    }

                    var take = int.Min(invItem.RemainingQuantity, needQuantity);
                    raidValue += take * invItem.EffectivePrice;
                    invItem.RemainingQuantity -= take;
                    needQuantity -= take;
                }
            }

            result.Add(new RaidCalculation
            {
                RaidId = raidInfo.Raid.Id,
                RawValue = raidValue,
                ParticipantIds = raidInfo.Participants.Select(p => p.ParticipantId).ToList()
            });
        }

        return result;
    }

    /// <summary>
    ///     Шаг 4-5: Пропорциональное распределение пула по рейдам и внутри рейдов
    /// </summary>
    private void DistributeByRaids(List<RaidCalculation> raidCalcs, decimal distributionPool)
    {
        _calculations = new Dictionary<Guid, MemberCalculation>();

        var totalRaidValue = raidCalcs.Sum(r => r.RawValue);
        if (totalRaidValue <= 0)
        {
            _undistributed = distributionPool;
            return;
        }

        // Пропорциональное распределение пула
        foreach (var raid in raidCalcs)
        {
            raid.DistributionPool = (raid.RawValue / totalRaidValue) * distributionPool;
        }

        // Распределение внутри рейдов
        var registeredUserIds = _members.Select(m => m.UserId).ToHashSet();
        _undistributed = 0m;

        foreach (var raid in raidCalcs)
        {
            if (raid.ParticipantIds.Count == 0)
            {
                continue;
            }

            var sharePerParticipant = raid.DistributionPool / raid.ParticipantIds.Count;

            foreach (var participantId in raid.ParticipantIds)
            {
                if (!registeredUserIds.Contains(participantId))
                {
                    _undistributed += sharePerParticipant;
                }

                var calc = GetOrCreateCalculation(participantId);
                calc.ActivityShare += sharePerParticipant;
                calc.RaidCount++;
            }
        }
    }

    /// <summary>
    ///     Получение информации о РБ инфузиях из SalaryLoot
    ///     AvailableSynthesisExp - доступный опыт синтеза (SUM of Quantity * SynthesisExp)
    ///     PricePerSynthesisExp - цена за 1 единицу опыта синтеза (PricePerItem / SynthesisExp)
    /// </summary>
    private (decimal AvailableSynthesisExp, decimal PricePerSynthesisExp) GetWorldBossInfusionInfo()
    {
        var wbLoot = _loot
            .Where(l => l.LootItem?.Type == LootType.WorldBossInfusion && l.LootItem.SynthesisExp > 0)
            .ToList();

        if (wbLoot.Count == 0)
        {
            return (0m, 0m);
        }

        // Суммарный доступный опыт синтеза
        var availableSynthesisExp = wbLoot.Sum(l => l.Quantity * (l.LootItem?.SynthesisExp ?? 0));

        // Цена за 1 единицу опыта синтеза (берём из первого попавшегося)
        var firstItem = wbLoot.First();
        var synthesisExp = firstItem.LootItem?.SynthesisExp ?? 1;
        var pricePerSynthesisExp = firstItem.PricePerItem / synthesisExp;

        return (availableSynthesisExp, pricePerSynthesisExp);
    }

    /// <summary>
    ///     Шаг 6: Сборка результатов расчёта по каждому участнику.
    ///     Не мутирует SalaryMember, а возвращает список SalaryMemberCalculationResult.
    ///     AmountWorldBossInfusion = опыт синтеза (goldShare / pricePerSynthesisExp)
    /// </summary>
    private List<SalaryMemberCalculationResult> BuildMemberResults(decimal pricePerSynthesisExp)
    {
        var totalRaids = _raids.Count;
        var results = new List<SalaryMemberCalculationResult>();

        foreach (var member in _members)
        {
            var calc = _calculations.GetValueOrDefault(member.UserId);
            var activityGold = calc?.ActivityShare ?? 0m;
            var responsibilityGold = calc?.ResponsibilityShare ?? 0m;
            var raidCount = calc?.RaidCount ?? 0;
            var totalGold = activityGold + responsibilityGold;

            var activityPercentage = totalRaids > 0
                ? (decimal)raidCount / totalRaids * 100
                : 0m;

            decimal amountGold;
            decimal amountWorldBossInfusion;

            switch (member.PaymentType)
            {
                case SalaryPaymentType.Gold:
                    amountGold = totalGold;
                    amountWorldBossInfusion = 0m;
                    break;

                case SalaryPaymentType.WorldBossInfusion:
                    if (pricePerSynthesisExp > 0)
                    {
                        amountWorldBossInfusion = totalGold / pricePerSynthesisExp;
                        amountGold = 0m;
                    }
                    else
                    {
                        amountGold = totalGold;
                        amountWorldBossInfusion = 0m;
                    }
                    break;

                default:
                    amountGold = totalGold;
                    amountWorldBossInfusion = 0m;
                    break;
            }

            results.Add(new SalaryMemberCalculationResult(
                member.UserId,
                activityPercentage,
                activityGold,
                responsibilityGold,
                amountGold,
                amountWorldBossInfusion,
                0m));
        }

        return results;
    }

    /// <summary>
    ///     Шаг 7: Балансировка РБ инфузий - если не хватает опыта синтеза, компенсируем золотом
    /// </summary>
    private List<SalaryMemberCalculationResult> BalanceWorldBossInfusions(
        List<SalaryMemberCalculationResult> memberResults,
        decimal availableSynthesisExp,
        decimal pricePerSynthesisExp)
    {
        if (pricePerSynthesisExp <= 0)
        {
            return memberResults;
        }

        var wbMemberIds = _members
            .Where(m => m.PaymentType == SalaryPaymentType.WorldBossInfusion)
            .Select(m => m.UserId)
            .ToHashSet();

        if (wbMemberIds.Count == 0)
        {
            return memberResults;
        }

        var totalRequestedExp = memberResults
            .Where(m => wbMemberIds.Contains(m.MemberId))
            .Sum(m => m.AmountWorldBossInfusion);

        if (totalRequestedExp <= availableSynthesisExp)
        {
            return memberResults;
        }

        // Не хватает опыта синтеза - пропорционально уменьшаем и компенсируем золотом
        var ratio = availableSynthesisExp / totalRequestedExp;

        return memberResults.Select(m =>
        {
            if (!wbMemberIds.Contains(m.MemberId))
            {
                return m;
            }

            var actualExp = m.AmountWorldBossInfusion * ratio;
            var deficitExp = m.AmountWorldBossInfusion - actualExp;
            var goldCompensation = deficitExp * pricePerSynthesisExp;

            return m with
            {
                AmountWorldBossInfusion = actualExp,
                AmountGold = goldCompensation
            };
        }).ToList();
    }

    /// <summary>
    ///     Распределение голды за ответственность по типам расходов
    /// </summary>
    private void DistributeResponsibilityGold()
    {
        foreach (var expense in _expenses)
        {
            switch (expense.ExpensesType)
            {
                case SalaryExpensesType.RaidLeader:
                    // TODO: Реализовать распределение для рейд-лидеров
                    break;

                case SalaryExpensesType.FillingJmg:
                    // BossType: Jmg (1), AbyssalJmg (2)
                    DistributeToRaidCreators(expense.Amount, [BossType.Jmg, BossType.AbyssalJmg]);
                    break;

                case SalaryExpensesType.FillingRagnoraAndMorpheus:
                    // BossType: Rangora (3), Morpheus (4)
                    DistributeToRaidCreators(expense.Amount, [BossType.Rangora, BossType.Morpheus]);
                    break;

                case SalaryExpensesType.FillingNehliyaAndRisopoda:
                    // BossType: Nehliya (11), Risopoda (12)
                    DistributeToRaidCreators(expense.Amount, [BossType.Nehliya, BossType.Risopoda]);
                    break;

                case SalaryExpensesType.FillingAbyssalSehekmet:
                    // BossType: AbyssalSehekmet (10)
                    DistributeToRaidCreators(expense.Amount, [BossType.AbyssalSehekmet]);
                    break;

                case SalaryExpensesType.FillingWb:
                    // Все остальные BossType (мировые боссы)
                    DistributeToRaidCreators(expense.Amount,
                    [
                        BossType.Kraken, BossType.BlackDragon, BossType.Charybdis,
                        BossType.Leviathan, BossType.Anthalon
                    ]);
                    break;

                case SalaryExpensesType.GuildBank:
                    // Ничего не делаем - отчисления в банк
                    break;

                case SalaryExpensesType.TargetMember:
                    // Напрямую добавляем конкретному участнику
                    if (!expense.UserId.HasValue)
                    {
                        break;
                    }

                    AddResponsibilityShare(expense.UserId.Value, expense.Amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    /// <summary>
    ///     Равномерно распределяет сумму между создателями рейдов указанных типов
    /// </summary>
    private void DistributeToRaidCreators(decimal amount, BossType[] bossTypes)
    {
        var creatorIds = _raids
            .Where(r => bossTypes.Contains(r.Raid.BossType))
            .Select(r => r.Raid.CreatorId)
            .Distinct()
            .ToList();

        if (creatorIds.Count == 0)
        {
            return;
        }

        var sharePerCreator = amount / creatorIds.Count;
        foreach (var creatorId in creatorIds)
        {
            AddResponsibilityShare(creatorId, sharePerCreator);
        }
    }

    /// <summary>
    ///     Добавляет голду за ответственность участнику
    /// </summary>
    private void AddResponsibilityShare(Guid userId, decimal amount)
    {
        var calc = GetOrCreateCalculation(userId);
        calc.ResponsibilityShare += amount;
    }

    /// <summary>
    ///     Получает или создаёт расчётные данные участника
    /// </summary>
    private MemberCalculation GetOrCreateCalculation(Guid userId)
    {
        if (_calculations.TryGetValue(userId, out var calc))
        {
            return calc;
        }

        calc = new MemberCalculation { UserId = userId };
        _calculations[userId] = calc;

        return calc;
    }

    /// <summary>
    ///     Расчётные данные участника
    /// </summary>
    private class MemberCalculation
    {
        /// <summary>
        ///     Идентификатор пользователя
        /// </summary>
        public required Guid UserId { get; init; }

        /// <summary>
        ///     Голда за активность (участие в рейдах)
        /// </summary>
        public decimal ActivityShare { get; set; }

        /// <summary>
        ///     Голда за ответственность
        /// </summary>
        public decimal ResponsibilityShare { get; set; }

        /// <summary>
        ///     Количество посещённых рейдов
        /// </summary>
        public int RaidCount { get; set; }
    }

    /// <summary>
    ///     Вспомогательная структура для расчёта рейдов
    /// </summary>
    private class RaidCalculation
    {
        public long RaidId { get; set; }
        public decimal RawValue { get; set; }
        public decimal DistributionPool { get; set; }
        public List<Guid> ParticipantIds { get; set; } = [];
    }

    /// <summary>
    ///     Элемент инвентаря лута для списания
    /// </summary>
    private class LootInventoryItem
    {
        public int LootId { get; init; }
        /// <summary>
        ///     Эффективная цена с учётом скидки: PricePerItem * (1 - DiscountPercent / 100)
        /// </summary>
        public decimal EffectivePrice { get; init; }
        public int RemainingQuantity { get; set; }
    }
}
