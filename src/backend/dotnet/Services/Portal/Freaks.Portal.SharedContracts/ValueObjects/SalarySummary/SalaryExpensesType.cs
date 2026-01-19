namespace Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

/// <summary>
///     Тип зарплатных расходов
/// </summary>
public enum SalaryExpensesType
{
    /// <summary>
    ///     ЗП рейд лидерам
    /// </summary>
    RaidLeader = 1,

    /// <summary>
    ///     Зп заполняющим АГЛ
    /// </summary>
    FillingJmg = 2,

    /// <summary>
    ///     ЗП заполняющим ВБ
    /// </summary>
    FillingWb = 3,

    /// <summary>
    ///     Отчисления в ги банк
    /// </summary>
    GuildBank = 4,

    /// <summary>
    ///     Поощрение конкретному участнику
    /// </summary>
    TargetMember = 100,
}