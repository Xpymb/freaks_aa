using System.Security.Claims;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Users.Bll;
using Freaks.Users.Contracts;
using Freaks.Users.Dal;
using Microsoft.AspNetCore.Http;

namespace Freaks.Users.Middlewares;

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
        if (user?.Identity?.IsAuthenticated == false)
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
                    Email = user!.FindFirst(ClaimTypes.Email)!.Value,
                    GameNickname = user!.FindFirst("game_nickname")!.Value,
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

    private static List<UserRole> GetUserRoles(ClaimsPrincipal user)
    {
        var roles = new List<UserRole>();

        foreach (var role in user.Claims.Where(c => c.Type == ClaimTypes.Role))
        {
            switch (role.Value)
            {
                case "guild_leader":
                    roles.Add(UserRole.GuildLeader);
                    break;
                case "admin":
                    roles.Add(UserRole.Admin);
                    break;
                case "editor":
                    roles.Add(UserRole.Editor);
                    break;
                case "member":
                    roles.Add(UserRole.Member);
                    break;
            }
        }

        return roles;
    }
}