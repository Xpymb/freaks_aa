namespace Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

/// <summary>
///     Статус заполнения зп
/// </summary>
public enum SalaryFillStepType
{
    /// <summary>
    ///     Параметры периода
    /// </summary>
    Parameters = 1,

    /// <summary>
    ///     Продано за период
    /// </summary>
    SoldByPeriod = 10,

    /// <summary>
    ///     Доля руководства
    /// </summary>
    GuildLeaderSalary = 20,

    /// <summary>
    ///     Расходы и отчисления
    /// </summary>
    Expenses = 30,

    /// <summary>
    ///     Итоговый отчёт
    /// </summary>
    FinalReports = 40,

    /// <summary>
    ///     Распределение зарплат участникам
    /// </summary>
    Members = 50,
}