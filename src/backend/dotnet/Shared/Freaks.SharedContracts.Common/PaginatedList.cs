namespace Freaks.SharedContracts.Common;

/// <summary>
///     Представляет постраничный результат выборки данных.
///     Используется для возврата данных с пагинацией в API и других слоях приложения.
/// </summary>
/// <typeparam name="T">Тип элементов внутри списка.</typeparam>
/// <param name="Items">Список элементов текущей страницы.</param>
/// <param name="Take">Количество элементов, запрошенное на страницу (page size).</param>
/// <param name="Skip">Количество пропущенных элементов (offset).</param>
/// <param name="TotalCount">Общее количество элементов без учёта пагинации.</param>
public record PaginatedList<T>(
    IList<T> Items,
    int? Take,
    int? Skip,
    int TotalCount);