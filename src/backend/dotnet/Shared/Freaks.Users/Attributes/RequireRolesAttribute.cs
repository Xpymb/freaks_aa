using Freaks.Users.Bll;
using Freaks.Users.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Freaks.Users.Attributes;

/// <summary>
///     Атрибут авторизации, ограничивающий доступ к действию или контроллеру на основе пользовательских ролей.
///     Проверяет, содержит ли текущий пользователь хотя бы одну из указанных ролей в <see cref="IUserContext" />.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireRolesAttribute : Attribute, IAuthorizationFilter
{
    private readonly HashSet<UserRole> _requiredRoles;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RequireRolesAttribute" />, указывая роли, доступ к которым разрешён.
    /// </summary>
    /// <param name="roles">Набор ролей, наличие хотя бы одной из которых требуется у пользователя.</param>
    public RequireRolesAttribute(params UserRole[] roles)
    {
        _requiredRoles = roles.ToHashSet();
    }

    /// <summary>
    ///     Метод авторизации, вызываемый во время обработки запроса.
    ///     Проверяет, есть ли в <see cref="IUserContext" /> хотя бы одна из разрешённых ролей.
    ///     В случае отсутствия ролей возвращает <see cref="ForbidResult" />.
    /// </summary>
    /// <param name="context">Контекст авторизации.</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;

        if (!httpContext.Items.TryGetValue("UserContext", out var value)
            || value is not IUserContext userContext)
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!userContext.Roles.Any(r => _requiredRoles.Contains(r)))
        {
            context.Result = new ForbidResult();
        }
    }
}