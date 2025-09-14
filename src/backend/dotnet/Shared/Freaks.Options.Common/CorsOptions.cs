using System.ComponentModel.DataAnnotations;

namespace Freaks.Options.Common;

/// <summary>
///     Настройки CORS для микросервисов.
///     Позволяет настраивать разрешенные origins, методы и заголовки.
/// </summary>
public class CorsOptions
{
    /// <summary>
    ///     Список разрешенных origins (источников запросов).
    ///     Если пустой, то разрешаются все origins (только для Development/Compose).
    /// </summary>
    public string[] AllowedOrigins { get; init; } = Array.Empty<string>();

    /// <summary>
    ///     Список разрешенных HTTP методов.
    ///     По умолчанию разрешены все методы.
    /// </summary>
    public string[] AllowedMethods { get; init; } = { "GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH" };

    /// <summary>
    ///     Список разрешенных заголовков.
    ///     По умолчанию разрешены все заголовки.
    /// </summary>
    public string[] AllowedHeaders { get; init; } = { "*" };

    /// <summary>
    ///     Список заголовков, которые клиент может получить в ответе.
    ///     По умолчанию включает Content-Disposition для файлов.
    /// </summary>
    public string[] ExposedHeaders { get; init; } = { "Content-Disposition" };

    /// <summary>
    ///     Разрешить ли credentials (cookies, authorization headers).
    ///     По умолчанию false для безопасности.
    /// </summary>
    public bool AllowCredentials { get; init; } = false;
}
