using System.Linq.Expressions;
using Freaks.Dal.Common.ValueObjects;

namespace Freaks.Dal.Common.Extensions;

/// <summary>
///     Предоставляет расширения для коллекций и запросов, включая пагинацию и сортировку.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    ///     Применяет пагинацию (Skip/Take) к <see cref="IQueryable{T}" />.
    /// </summary>
    /// <typeparam name="T">Тип элементов в источнике.</typeparam>
    /// <param name="queryable">Исходный запрос.</param>
    /// <param name="take">Количество элементов для возврата.</param>
    /// <param name="skip">Количество элементов для пропуска.</param>
    /// <returns>Запрос с применённой пагинацией.</returns>
    public static IQueryable<T> UseTakeSkip<T>(
        this IQueryable<T> queryable,
        int? take = null,
        int? skip = null)
        where T : class
    {
        if (take is not null
            && skip is not null)
        {
            return queryable
                   .Skip(skip.Value)
                   .Take(take.Value);
        }

        if (take is not null)
        {
            return queryable.Take(take.Value);
        }

        if (skip is not null)
        {
            return queryable.Skip(skip.Value);
        }

        return queryable;
    }

    /// <summary>
    ///     Применяет пагинацию (Skip/Take) к <see cref="IEnumerable{T}" />.
    /// </summary>
    /// <typeparam name="T">Тип элементов в источнике.</typeparam>
    /// <param name="collection">Исходная коллекция.</param>
    /// <param name="take">Количество элементов для возврата.</param>
    /// <param name="skip">Количество элементов для пропуска.</param>
    /// <returns>Коллекция с применённой пагинацией.</returns>
    public static IEnumerable<T> UseTakeSkip<T>(
        this IEnumerable<T> collection,
        int? take = null,
        int? skip = null)
        where T : class
    {
        if (take is not null
            && skip is not null)
        {
            return collection.Skip(skip.Value)
                             .Take(take.Value);
        }

        if (take is not null)
        {
            return collection.Take(take.Value);
        }

        if (skip is not null)
        {
            return collection.Skip(skip.Value);
        }

        return collection;
    }

    /// <summary>
    ///     Применяет сортировку к <see cref="IQueryable{T}" /> по заданному ключу и направлению.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
    /// <typeparam name="TSource">Тип элемента.</typeparam>
    /// <param name="queryable">Исходный запрос.</param>
    /// <param name="keySelector">Выражение выбора ключа сортировки.</param>
    /// <param name="mode">Режим сортировки (Asc, Desc, None).</param>
    /// <returns>Отсортированный запрос.</returns>
    public static IOrderedQueryable<TSource> OrderBy<TKey, TSource>(
        this IQueryable<TSource> queryable,
        Expression<Func<TSource, TKey>> keySelector,
        OrderByMode mode)
    {
        return mode switch
        {
            OrderByMode.None => queryable.OrderBy(_ => 1),
            OrderByMode.Asc => queryable.OrderBy(keySelector),
            OrderByMode.Desc => queryable.OrderByDescending(keySelector),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };
    }

    /// <summary>
    ///     Применяет последующую сортировку (ThenBy) к <see cref="IOrderedQueryable{T}" /> по заданному ключу и направлению.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
    /// <typeparam name="TSource">Тип элемента.</typeparam>
    /// <param name="queryable">Упорядоченный запрос.</param>
    /// <param name="keySelector">Выражение выбора ключа сортировки.</param>
    /// <param name="mode">Режим сортировки.</param>
    /// <returns>Дополнительно отсортированный запрос.</returns>
    public static IOrderedQueryable<TSource> ThenBy<TKey, TSource>(
        this IOrderedQueryable<TSource> queryable,
        Expression<Func<TSource, TKey>> keySelector,
        OrderByMode mode)
    {
        return mode switch
        {
            OrderByMode.None => queryable,
            OrderByMode.Asc => queryable.ThenBy(keySelector),
            OrderByMode.Desc => queryable.ThenByDescending(keySelector),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };
    }

    /// <summary>
    ///     Применяет сортировку к <see cref="IEnumerable{T}" /> по заданному ключу и направлению.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
    /// <typeparam name="TSource">Тип элемента.</typeparam>
    /// <param name="collection">Исходная коллекция.</param>
    /// <param name="keySelector">Функция выбора ключа сортировки.</param>
    /// <param name="mode">Режим сортировки.</param>
    /// <returns>Отсортированная коллекция.</returns>
    public static IOrderedEnumerable<TSource> OrderBy<TKey, TSource>(
        this IEnumerable<TSource> collection,
        Func<TSource, TKey> keySelector,
        OrderByMode mode)
    {
        return mode switch
        {
            OrderByMode.None => collection.OrderBy(_ => 1),
            OrderByMode.Asc => collection.OrderBy(keySelector),
            OrderByMode.Desc => collection.OrderByDescending(keySelector),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };
    }

    /// <summary>
    ///     Применяет последующую сортировку (ThenBy) к <see cref="IOrderedEnumerable{T}" /> по заданному ключу и направлению.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
    /// <typeparam name="TSource">Тип элемента.</typeparam>
    /// <param name="collection">Упорядоченная коллекция.</param>
    /// <param name="keySelector">Функция выбора ключа сортировки.</param>
    /// <param name="mode">Режим сортировки.</param>
    /// <returns>Дополнительно отсортированная коллекция.</returns>
    public static IOrderedEnumerable<TSource> ThenBy<TKey, TSource>(
        this IOrderedEnumerable<TSource> collection,
        Func<TSource, TKey> keySelector,
        OrderByMode mode)
    {
        return mode switch
        {
            OrderByMode.None => collection,
            OrderByMode.Asc => collection.ThenBy(keySelector),
            OrderByMode.Desc => collection.ThenByDescending(keySelector),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };
    }
}