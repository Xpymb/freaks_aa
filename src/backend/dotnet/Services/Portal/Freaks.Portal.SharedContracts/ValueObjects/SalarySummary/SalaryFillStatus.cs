namespace Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

/// <summary>
///     Статус заполнения зп
/// </summary>
public enum SalaryFillStatus
{
    /// <summary>
    ///     Параметры периода
    /// </summary>
    PeriodParameters = 1,

    /// <summary>
    ///     Продано за период
    /// </summary>
    SoldByPeriod = 2,

    /// <summary>
    ///     Доля руководства
    /// </summary>
    GuildLeaderSalary = 3,

    /// <summary>
    ///     Расходы и отчисления
    /// </summary>
    Expenses = 4,

    /// <summary>
    ///     Итоговый отчёты
    /// </summary>
    FinalReports = 5,
}