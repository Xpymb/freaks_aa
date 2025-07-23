using System.ComponentModel.DataAnnotations;

namespace Freaks.Options.Common;

/// <summary>
///     Настройки подключения к хранилищу объектов (например, MinIO или Amazon S3).
/// </summary>
public class StorageOptions
{
    /// <summary>
    ///     Хост-адрес сервиса хранилища (например, https://s3.example.com).
    /// </summary>
    [Required]
    public required string Host { get; init; }

    /// <summary>
    ///     Ключ доступа (Access Key) для авторизации в сервисе хранилища.
    /// </summary>
    [Required]
    public required string AccessKey { get; init; }

    /// <summary>
    ///     Секретный ключ (Secret Key) для авторизации в сервисе хранилища.
    /// </summary>
    [Required]
    public required string SecretKey { get; init; }

    /// <summary>
    ///     Название корзины (bucket), используемой по умолчанию для загрузки и хранения объектов.
    /// </summary>
    [Required]
    public required string DefaultBucketName { get; init; }

    /// <summary>
    ///     Регион хранилища, необходимый для некоторых провайдеров (например, AWS).
    /// </summary>
    [Required]
    public required string Region { get; init; }
}