using Freaks.Users.SharedContracts.Dto;
using Freaks.Users.SharedContracts.Requests;

namespace Freaks.Users.Bll.Interfaces;

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
    Task<UserDto> GetAsync(Guid id);

    /// <summary>
    ///     Возвращает список всех пользователей.
    /// </summary>
    /// <param name="includeWoRoles">Показывать пользователей без ролей.</param>
    /// <returns>Список объектов <see cref="UserDto" />.</returns>
    Task<IList<UserDto>> GetListAsync(bool includeWoRoles);

    /// <summary>
    ///     Обновляет роли пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="request">Запрос, содержащий список новых ролей пользователя.</param>
    /// <returns>Обновлённый пользователь с актуальными ролями.</returns>
    Task<UserDto> UpdateRolesAsync(Guid userId, UpdateUserRolesRequest request);
}