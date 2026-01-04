using Freaks.Dal.Common.ValueObjects;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;

/// <summary>
///     Запрос на получение списка зарплатных периодов с фильтрацией и сортировкой.
/// </summary>
/// <param name="From">Начальная дата диапазона (включительно).</param>
/// <param name="To">Конечная дата диапазона (включительно).</param>
/// <param name="SortBy">Поле, по которому нужно отсортировать результаты.</param>
/// <param name="SortMode">Направление сортировки (по возрастанию или убыванию).</param>
/// <param name="Take">Сколько записей вывести.</param>
/// <param name="Skip">Сколько записей пропустить.</param>
public record GetSalaryListRequest(
    DateOnly? From,
    DateOnly? To,
    SalaryListSortByType SortBy,
    OrderByMode SortMode,
    int? Take,
    int? Skip);
