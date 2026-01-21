using Microsoft.EntityFrameworkCore;

namespace Freaks.Dal.Common.Interfaces;

/// <summary>
///     Интерфейс единицы работы (Unit of Work), предоставляющий абстракцию для выполнения операций с базой данных в рамках
///     одного контекста.
///     Гарантирует, что все действия выполняются в единой транзакции при необходимости.
/// </summary>
/// <typeparam name="TContext">
///     Тип контекста базы данных, наследующий <see cref="DbContext" /> и реализующий
///     <see cref="IBaseDbContext" />.
/// </typeparam>
public interface IUnitOfWork<out TContext>
    where TContext : DbContext, IBaseDbContext
{
    /// <summary>
    ///     Выполняет асинхронную операцию без параметров и возвращаемого результата в транзакции.
    /// </summary>
    /// <param name="operation">Асинхронная операция, которую необходимо выполнить.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task<TResult> ExecuteInsideTransactionAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Выполняет асинхронную операцию без параметров, возвращающую результат, в транзакции.
    /// </summary>
    /// <typeparam name="TResult">Тип возвращаемого результата.</typeparam>
    /// <param name="operation">Асинхронная операция, которую необходимо выполнить.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения операции.</returns>
    Task ExecuteInsideTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Выполняет асинхронную операцию над контекстом базы данных.
    /// </summary>
    /// <param name="operation">Функция, принимающая контекст и выполняющая операцию.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Задача, представляющая выполнение операции.</returns>
    Task ExecuteInsideTransactionAsync(Func<TContext, Task> operation, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Выполняет асинхронную операцию над контекстом базы данных и возвращает результат.
    /// </summary>
    /// <typeparam name="TResult">Тип возвращаемого результата.</typeparam>
    /// <param name="operation">Функция, принимающая контекст и возвращающая результат операции.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения операции.</returns>
    Task<TResult> ExecuteInsideTransactionAsync<TResult>(Func<TContext, Task<TResult>> operation, CancellationToken cancellationToken = default);
}