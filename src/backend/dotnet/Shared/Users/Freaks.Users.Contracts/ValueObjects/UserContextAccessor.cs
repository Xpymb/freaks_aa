using Freaks.WebApi.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Freaks.Users.Contracts.ValueObjects;

/// <summary>
///     Предоставляет доступ к контексту текущего пользователя,
///     который был заранее установлен в <see cref="HttpContext.Items" /> через middleware.
/// </summary>
public class UserContextAccessor : IUserContext
{
    /// <inheritdoc />
    public Guid Id { get; init; }

    /// <inheritdoc />
    public List<UserRole> Roles { get; init; }

    /// <inheritdoc />
    public string Username { get; init; }

    /// <inheritdoc />
    public string Email { get; init; }

    /// <inheritdoc />
    public string GameNickname { get; init; }

    /// <inheritdoc />
    public DateTimeOffset CreatedDt { get; init; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedDt { get; init; }

    /// <summary>
    ///     Конструктор, который инициализирует контекст пользователя из текущего <see cref="HttpContext" />.
    /// </summary>
    /// <param name="httpContextAccessor">Объект для доступа к текущему <see cref="HttpContext" />.</param>
    /// <exception cref="UnauthorizedApiException">
    ///     Выбрасывается, если контекст пользователя не найден в <see cref="HttpContext.Items" />
    ///     или если пользователь не авторизован.
    /// </exception>
    public UserContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;

        if ((context?.Items.TryGetValue("UserContext", out var value) == true)
            && value is IUserContext uc)
        {
            Id = uc.Id;
            Roles = uc.Roles;
            Username = uc.Username;
            Email = uc.Email;
            GameNickname = uc.GameNickname;
            CreatedDt = uc.CreatedDt;
            UpdatedDt = uc.UpdatedDt;
        }
        else
        {
            throw new UnauthorizedApiException();
        }
    }
}