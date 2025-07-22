namespace Freaks.Portal.SharedContracts.Requests.RaidSummary;

/// <summary>
///     Поля, по которым может осуществляться сортировка списка рейдов.
///     Используется в параметрах фильтрации и сортировки при получении рейдов.
/// </summary>
public enum RaidListSortByType
{
    /// <summary>
    ///     Сортировка по дате начала рейда.
    /// </summary>
    StartDt = 0,

    /// <summary>
    ///     Сортировка по статусу рейда.
    /// </summary>
    RaidStatus = 1,

    /// <summary>
    ///     Сортировка по дате создания рейда.
    /// </summary>
    CreatedDt = 2,
}