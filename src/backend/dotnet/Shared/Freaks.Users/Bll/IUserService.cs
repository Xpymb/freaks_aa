using Freaks.Users.Contracts;

namespace Freaks.Users.Bll;

/// <summary>
///     Сервис для работы с пользователями.
///     Предоставляет методы получения, создания и обновления пользователей.
/// </summary>
public interface IUserService
{
    /// <summary>
    ///     Получает пользователя по его идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор пользователя.</param>
    /// <returns>Объект пользователя.</returns>
    Task<User> GetAsync(Guid id);

    /// <summary>
    ///     Создаёт нового пользователя.
    /// </summary>
    /// <param name="user">Объект пользователя для создания.</param>
    /// <returns>Созданный пользователь.</returns>
    Task<User> CreateAsync(User user);

    /// <summary>
    ///     Обновляет данные существующего пользователя.
    /// </summary>
    /// <param name="user">Объект пользователя с обновлёнными данными.</param>
    /// <returns>Обновлённый пользователь.</returns>
    Task<User> UpdateAsync(User user);
}