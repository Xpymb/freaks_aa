using System.Security.Claims;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Users.Contracts.Attributes;
using Freaks.Users.Contracts.Entities;
using Freaks.Users.Contracts.Extensions;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.Users.Dal.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Freaks.Users.Common.Middlewares;

/// <summary>
///     Middleware, отвечающий за инициализацию контекста пользователя на основе информации из токена.
///     Загружает данные пользователя из базы и сохраняет их в <see cref="HttpContext.Items" />,
///     чтобы они были доступны в дальнейшем через <see cref="IUserContext" />.
/// </summary>
public class UserContextMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UserContextMiddleware" />.
    /// </summary>
    /// <param name="next">Делегат для вызова следующего middleware в конвейере.</param>
    public UserContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///     Выполняет middleware-логику. Извлекает идентификатор пользователя из клейма `sid`,
    ///     загружает его из провайдера и помещает в <see cref="HttpContext.Items" />.
    /// </summary>
    /// <param name="context">Текущий HTTP-контекст запроса.</param>
    /// <param name="userProvider">Провайдер пользователей для загрузки данных из базы.</param>
    public async Task InvokeAsync(HttpContext context, IUserProvider userProvider)
    {
        var user = context.User;
        if (user.Identity?.IsAuthenticated == false)
        {
            await _next(context);
            return;
        }

        var sidClaim =
            user?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;

        if (sidClaim is null)
        {
            await _next(context);
            return;
        }

        var userId = Guid.Parse(sidClaim);

        var userEntity = await userProvider.GetAsync(userId, EntityTrackingType.NoTracking);
        if (userEntity is null)
        {
            userEntity =
                new User
                {
                    Id = userId,
                    Roles = GetUserRoles(user!),
                    Username = user!.FindFirst("preferred_username")!.Value,
                    Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                    GameNickname = user.FindFirst("game_nickname")?.Value ?? "",
                    CreatedDt = DateTimeOffset.UtcNow,
                };

            await userProvider.CreateAsync(userEntity);
        }

        var userContext =
            new UserContext
            {
                Id = userEntity.Id,
                Roles = userEntity.Roles,
                Username = userEntity.Username,
                Email = userEntity.Email,
                GameNickname = userEntity.GameNickname,
                CreatedDt = userEntity.CreatedDt,
                UpdatedDt = userEntity.UpdatedDt,
            };

        context.Items["UserContext"] = userContext;

        await _next(context);
    }

    /// <summary>
    ///     Извлекает список ролей <see cref="UserRole" /> из коллекции клаймов пользователя,
    ///     сопоставляя значения ролей из <see cref="ClaimTypes.Role" /> с атрибутом <see cref="UserRoleNameAttribute" />.
    /// </summary>
    /// <param name="user">Объект <see cref="ClaimsPrincipal" />, содержащий клаймы пользователя.</param>
    /// <returns>Список ролей <see cref="UserRole" />, соответствующих найденным клаймам.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Если одно из значений клайма <see cref="ClaimTypes.Role" /> не сопоставляется ни с одной ролью.
    /// </exception>
    private static List<UserRole> GetUserRoles(ClaimsPrincipal user)
    {
        var roles = new List<UserRole>();
        
        foreach (var claim in user.Claims)
        {
            var userRole = UserRoleExtensions.GetRoleType(claim.Value);
            if (userRole is null
                || ((int)userRole == 0))
            {
                continue;
            }

            roles.Add(userRole.Value);
        }

        return roles;
    }
}