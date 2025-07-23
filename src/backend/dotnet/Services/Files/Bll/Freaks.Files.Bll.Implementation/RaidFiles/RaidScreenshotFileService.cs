using Freaks.Files.Bll.Implementation.Helpers;
using Freaks.Files.Bll.Interfaces.RaidFiles;
using Freaks.Files.Dal.Interfaces;
using Freaks.Files.SharedContracts.Dto;
using Freaks.Files.SharedContracts.Requests;
using Freaks.Options.Common;
using Microsoft.Extensions.Options;

namespace Freaks.Files.Bll.Implementation.RaidFiles;

/// <summary>
///     Сервис обработки скриншотов рейдов.
///     Отвечает за генерацию имени, определение пути и загрузку файла в объектное хранилище.
/// </summary>
public class RaidScreenshotFileService : IRaidFileService
{
    private readonly IStorageProvider _storageProvider;
    private readonly StorageOptions _storageOptions;

    /// <summary>
    ///     Создаёт новый экземпляр <see cref="RaidScreenshotFileService" />.
    /// </summary>
    /// <param name="storageProvider">Провайдер хранилища, используемый для операций с файлами.</param>
    /// <param name="storageOptions">Настройки подключения к хранилищу объектов.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если передан <c>null</c> в параметры конструктора.</exception>
    public RaidScreenshotFileService(IStorageProvider storageProvider, IOptions<StorageOptions> storageOptions)
    {
        _storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
        _storageOptions = storageOptions.Value ?? throw new ArgumentNullException(nameof(storageOptions));
    }

    /// <inheritdoc />
    public async Task<FileDto> UploadAsync(FileUploadRequest request, int raidId)
    {
        var filePath = GetFilePath(raidId);
        var fileName = StorageHelper.GenerateFileName(request.FileName);

        await _storageProvider.UploadAsync(request.Content, filePath, fileName, request.ContentType);

        return new FileDto(
            StorageHelper.GetFileUrl(_storageOptions.Host, _storageOptions.DefaultBucketName, filePath, fileName),
            StorageHelper.GetFileUri(_storageOptions.DefaultBucketName, filePath, fileName),
            fileName,
            request.ContentType);
    }

    /// <summary>
    ///     Формирует путь хранения скриншота на основе идентификатора рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Виртуальный путь к папке со скриншотами рейда.</returns>
    private static string GetFilePath(int raidId)
    {
        return $"raids/{raidId}/screenshots";
    }
}