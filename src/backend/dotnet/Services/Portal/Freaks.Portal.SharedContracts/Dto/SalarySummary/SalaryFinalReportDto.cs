namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     DTO итогового отчёта зарплатного периода.
/// </summary>
/// <param name="TotalGold">Общий доход в золоте за период.</param>
/// <param name="TotalWorldBossInfusion">Общий доход в опыте синтеза инфузий мирового босса.</param>
/// <param name="TotalErenorInfusion">Общий доход в опыте синтеза эренорских инфузий.</param>
/// <param name="GoldGuildLeaderExpenses">Золото, забранное руководством гильдии.</param>
/// <param name="WorldBossInfusionGuildLeaderExpenses">Инфузии мирового босса, забранные руководством.</param>
/// <param name="ErenorInfusionGuildLeaderExpenses">Эренорские инфузии, забранные руководством.</param>
/// <param name="GoldExpenses">Расходы гильдии в золоте.</param>
/// <param name="WorldBossInfusionExpenses">Расходы гильдии в инфузиях мирового босса.</param>
/// <param name="ErenorInfusionExpenses">Расходы гильдии в эренорских инфузиях.</param>
/// <param name="GoldForSale">Золото, оставшееся на продажу.</param>
/// <param name="WorldBossInfusionForSale">Инфузии мирового босса, оставшиеся на продажу.</param>
/// <param name="ErenorInfusionForSale">Эренорские инфузии, оставшиеся на продажу.</param>
public record SalaryFinalReportDto(
    decimal TotalGold,
    decimal TotalWorldBossInfusion,
    decimal TotalErenorInfusion,
    decimal GoldGuildLeaderExpenses,
    decimal WorldBossInfusionGuildLeaderExpenses,
    decimal ErenorInfusionGuildLeaderExpenses,
    decimal GoldExpenses,
    decimal WorldBossInfusionExpenses,
    decimal ErenorInfusionExpenses,
    decimal GoldForSale,
    decimal WorldBossInfusionForSale,
    decimal ErenorInfusionForSale);
