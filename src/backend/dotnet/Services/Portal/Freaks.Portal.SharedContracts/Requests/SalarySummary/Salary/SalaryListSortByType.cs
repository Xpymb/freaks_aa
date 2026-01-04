namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;

/// <summary>
///     Поля, по которым может осуществляться сортировка списка зарплатных периодов.
/// </summary>
public enum SalaryListSortByType
{
    /// <summary>
    ///     Сортировка по идентификатору.
    /// </summary>
    Id = 0,

    /// <summary>
    ///     Сортировка по названию зарплатного периода.
    /// </summary>
    Name = 1,

    /// <summary>
    ///     Сортировка по статусу заполнения.
    /// </summary>
    FillStatus = 2,

    /// <summary>
    ///     Сортировка по статусу регистрации.
    /// </summary>
    RegistrationStatus = 3,
}
