using System.ComponentModel.DataAnnotations;

namespace Freaks.Options.Common;

/// <summary>
///     Параметры подключения OAuth-провайдера, такие, как Keycloak или другой OpenID Connect сервер.
///     Используются для настройки аутентификации через JWT.
/// </summary>
public class AuthOptions
{
    /// <summary>
    ///     URL-адрес провайдера, который выпустил токен (Issuer).
    ///     Обычно выглядит как: https://auth.example.com/realms/myrealm
    /// </summary>
    [Required]
    public string? Issuer { get; init; }

    /// <summary>
    ///     Идентификатор клиента (ClientId), зарегистрированного в OAuth-провайдере.
    ///     Используется для валидации токена и определения разрешённых получателей.
    /// </summary>
    [Required]
    public string? ClientId { get; init; }
}