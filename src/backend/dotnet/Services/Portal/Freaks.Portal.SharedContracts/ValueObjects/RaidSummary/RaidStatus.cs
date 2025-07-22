namespace Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;

/// <summary>
///     Статус рейда
/// </summary>
public enum RaidStatus
{
    /// <summary>
    ///     Запланирован
    /// </summary>
    Planned = 1,

    /// <summary>
    ///     В ожидании скриншота
    /// </summary>
    WaitingScreenshot = 2,

    /// <summary>
    ///     В ожидании подтверждения
    /// </summary>
    WaitingSubmit = 3,

    /// <summary>
    ///     Завершён
    /// </summary>
    Ended = 4,
}