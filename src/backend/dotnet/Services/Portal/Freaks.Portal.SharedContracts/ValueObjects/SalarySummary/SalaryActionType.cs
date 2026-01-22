namespace Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

/// <summary>
///     Тип действия над зарплатным периодом
/// </summary>
public enum SalaryActionType
{
    /// <summary>
    ///     Заполнение данных зарплатного периода
    /// </summary>
    Fill = 1,

    /// <summary>
    ///     Регистрация участников на зарплату
    /// </summary>
    Register = 10,
}