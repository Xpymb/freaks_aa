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
    FillingJmg = 10,

    /// <summary>
    ///     Зп заполняющим Марли и Морфеуса
    /// </summary>
    FillingRagnoraAndMorpheus = 11,

    /// <summary>
    ///     Зп заполняющим Калеиль и Жука
    /// </summary>
    FillingNehliyaAndRisopoda = 12,

    /// <summary>
    ///     Зп заполняющим Кошку
    /// </summary>
    FillingAbyssalSehekmet = 13,

    /// <summary>
    ///     ЗП заполняющим ВБ
    /// </summary>
    FillingWb = 20,

    /// <summary>
    ///     Отчисления в ги банк
    /// </summary>
    GuildBank = 50,

    /// <summary>
    ///     Поощрение конкретному участнику
    /// </summary>
    TargetMember = 100,
}