namespace Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

/// <summary>
///     Статус регистрации на зп
/// </summary>
public enum SalaryRegistrationStatus
{
    /// <summary>
    ///     Ещё не открыта
    /// </summary>
    NotStarted = 1,

    /// <summary>
    ///     Открыта
    /// </summary>
    Opened = 2,

    /// <summary>
    ///     Завершена
    /// </summary>
    Ended = 3,
}