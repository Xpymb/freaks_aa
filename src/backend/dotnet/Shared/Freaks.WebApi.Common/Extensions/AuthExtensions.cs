using Freaks.Options.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Freaks.WebApi.Common.Extensions;

/// <summary>
///     Расширения для настройки авторизации.
/// </summary>
public static class AuthExtensions
{
    /// <summary>
    ///     Регистрирует аутентификацию через Keycloak с использованием JWT Bearer схемы.
    ///     Использует параметры из конфигурации, определённые в <see cref="AuthOptions" />.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
    /// <param name="configuration">Конфигурация приложения (например, из <c>appsettings.json</c>).</param>
    /// <returns>Обновлённая коллекция сервисов <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddKeycloakAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions =
            configuration
                .GetSection(nameof(AuthOptions))
                .Get<AuthOptions>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = authOptions!.Issuer;
                options.Audience = authOptions.ClientId;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuer = authOptions!.Issuer,
                        NameClaimType = "preferred_username",
                        RoleClaimType = "roles",
                    };
            });

        services.AddAuthorization();

        return services;
    }
}