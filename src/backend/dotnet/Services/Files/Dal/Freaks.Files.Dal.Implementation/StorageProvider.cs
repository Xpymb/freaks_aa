using Freaks.Files.Dal.Interfaces;
using Freaks.Options.Common;
using Freaks.WebApi.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Freaks.Files.Dal.Implementation;

/// <summary>
///     Провайдер хранилища, реализующий загрузку, получение и удаление файлов через MinIO.
///     Использует конфигурацию <see cref="StorageOptions" /> и может автоматически создавать bucket в средах разработки.
/// </summary>
public class StorageProvider : IStorageProvider
{
    private readonly IMinioClient _minioClient;
    private readonly IWebHostEnvironment _env;
    private readonly StorageOptions _options;

    /// <summary>
    ///     Создаёт экземпляр <see cref="StorageProvider" />.
    /// </summary>
    /// <param name="minioClient">Клиент MinIO для выполнения операций с хранилищем.</param>
    /// <param name="env">Информация о текущем окружении приложения.</param>
    /// <param name="options">Настройки хранилища, включая endpoint, ключи и default bucket.</param>
    /// <exception cref="ArgumentNullException">Если передан <c>null</c> в любой из параметров.</exception>
    public StorageProvider(IMinioClient minioClient, IWebHostEnvironment env, IOptions<StorageOptions> options)
    {
        _minioClient = minioClient ?? throw new ArgumentNullException(nameof(minioClient));
        _env = env ?? throw new ArgumentNullException(nameof(env));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public async Task<Stream> GetAsync(string absolutePath, string? bucket = null)
    {
        bucket ??= _options.DefaultBucketName;

        var ms = new MemoryStream();

        var args =
            new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(absolutePath)
                .WithCallbackStream(stream => stream.CopyTo(ms));

        await _minioClient.GetObjectAsync(args);
        ms.Position = 0;

        return ms;
    }

    /// <inheritdoc />
    public Task UploadAsync(Stream fileStream, string path, string fileName, string contentType, string? bucket = null)
    {
        bucket ??= _options.DefaultBucketName;
        //await EnsureBucketExistsAsync(bucket);

        var objectName = CombinePath(path, fileName);

        var args =
            new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);

        return _minioClient.PutObjectAsync(args);
    }

    /// <inheritdoc />
    public Task DeleteAsync(string absolutePath, string? bucket = null)
    {
        bucket ??= _options.DefaultBucketName;

        var args =
            new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(absolutePath);

        return _minioClient.RemoveObjectAsync(args);
    }

    /// <summary>
    ///     Проверяет наличие указанного bucket'а и создаёт его, если он отсутствует.
    ///     Автоматическое создание происходит только в окружениях Development или Compose.
    /// </summary>
    /// <param name="bucket">Имя bucket'а.</param>
    private async Task EnsureBucketExistsAsync(string bucket)
    {
        if (!_env.IsDevelopment()
            && !_env.IsCompose())
        {
            return;
        }

        var exists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
        if (!exists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
        }
    }

    /// <summary>
    ///     Объединяет путь и имя файла в корректный object key для MinIO.
    /// </summary>
    /// <param name="path">Виртуальная директория внутри bucket'а.</param>
    /// <param name="fileName">Имя файла.</param>
    /// <returns>Корректный ключ объекта (например, <c>dir/subdir/file.png</c>).</returns>
    private static string CombinePath(string path, string fileName)
    {
        return string.IsNullOrWhiteSpace(path)
            ? fileName
            : $"{path.TrimEnd('/')}/{fileName}";
    }
}