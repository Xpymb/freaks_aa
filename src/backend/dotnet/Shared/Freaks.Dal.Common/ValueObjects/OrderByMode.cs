namespace Freaks.Dal.Common.ValueObjects;

/// <summary>
///     Направление сортировки для сущностей при выполнении запросов.
///     Используется для указания способа упорядочивания данных.
/// </summary>
public enum OrderByMode
{
    /// <summary>
    ///     Без сортировки (значение по умолчанию).
    /// </summary>
    None = 0,

    /// <summary>
    ///     Сортировка по возрастанию.
    /// </summary>
    Asc = 1,

    /// <summary>
    ///     Сортировка по убыванию.
    /// </summary>
    Desc = 2,
}