using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Freaks.Dal.Common.Interfaces;

/// <summary>
///     Базовый интерфейс для контекста данных.
///     Содержит общие методы и свойства, необходимые для работы с базой данных.
/// </summary>
public interface IBaseDbContext
{
    /// <summary>
    ///     Асинхронно сохраняет все изменения, в базу данных.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены, позволяющий прервать операцию сохранения.</param>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Предоставляет доступ к объекту <see cref="DatabaseFacade" />,
    ///     который предоставляет API для взаимодействия с базой данных на низком уровне
    ///     (например, транзакции, выполнение SQL-команд и т.д.).
    /// </summary>
    DatabaseFacade Database { get; }
}