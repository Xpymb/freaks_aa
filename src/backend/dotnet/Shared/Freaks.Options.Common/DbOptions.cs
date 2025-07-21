using System.ComponentModel.DataAnnotations;

namespace Freaks.Options.Common;

/// <summary>
///     Настройки подключения к базе данных. Используются для конфигурации контекста данных (DbContext).
/// </summary>
public class DbOptions
{
    /// <summary>
    ///     Строка подключения к базе данных.
    /// </summary>
    [Required]
    public string? ConnectionString { get; init; }
}