using Freaks.Users.Bll.Interfaces;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.Users.SharedContracts.Dto;
using Freaks.Users.SharedContracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers;

/// <summary>
///     Контроллер для управления пользователями.
///     Предоставляет API для получения списка пользователей, информации о конкретном пользователе и обновления его ролей.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера <see cref="UsersController" />.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public UsersController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    /// <summary>
    ///     Получает информацию о пользователе по его идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор пользователя.</param>
    /// <returns>Объект <see cref="UserDto" /> с данными пользователя.</returns>
    [RequireRoles(UserRole.GuildLeader, UserRole.Admin)]
    [HttpGet("{id:Guid}")]
    public Task<UserDto> GetAsync([FromRoute] Guid id)
    {
        return _userService.GetAsync(id);
    }

    /// <summary>
    ///     Получает список всех пользователей.
    /// </summary>
    /// <param name="includeWoRoles">Флаг, указывающий, включать ли пользователей без назначенных ролей.</param>
    /// <returns>Список объектов <see cref="UserDto" />.</returns>
    [HttpGet]
    public Task<IList<UserDto>> GetListAsync([FromQuery] bool includeWoRoles)
    {
        return _userService.GetListAsync(includeWoRoles);
    }

    /// <summary>
    ///     Обновляет роли пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="request">Запрос, содержащий список новых ролей.</param>
    /// <returns>Обновлённый <see cref="UserDto" /> с актуальными ролями.</returns>
    [RequireRoles(UserRole.GuildLeader, UserRole.Admin)]
    [HttpPatch("{id:Guid}/roles")]
    public Task<UserDto> UpdateRolesAsync([FromRoute] Guid id, [FromBody] UpdateUserRolesRequest request)
    {
        return _userService.UpdateRolesAsync(id, request);
    }
}