using System.ComponentModel.DataAnnotations;

namespace Freaks.Options.Common;

/// <summary>
///     Параметры подключения к Redis
/// </summary>
public class RedisOptions
{
    /// <summary>
    ///     Хост Redis-сервера (например, "localhost" или "redis.my-domain.local")
    /// </summary>
    [Required]
    public string? Host { get; set; }

    /// <summary>
    ///     Порт Redis-сервера (по умолчанию 6379)
    /// </summary>
    [Required]
    public int? Port { get; set; } = 6379;

    /// <summary>
    ///     Пароль доступа к Redis (если настроен)
    /// </summary>
    [Required]
    public string? Password { get; set; }

    /// <summary>
    ///     Порядковый номер базы данных
    /// </summary>
    [Required]
    public int? Database { get; set; }

    /// <summary>
    ///     Таймаут подключения (в миллисекундах)
    /// </summary>
    public int ConnectTimeout { get; set; } = 5000;

    /// <summary>
    ///     Таймаут синхронизации (в миллисекундах)
    /// </summary>
    public int SyncTimeout { get; set; } = 10000;
}