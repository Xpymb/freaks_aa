using Freaks.Files.SharedContracts.Dto;
using Freaks.Files.SharedContracts.Requests;

namespace Freaks.Files.Bll.Interfaces.RaidFiles;

/// <summary>
///     Сервис для работы с файлами, связанными с рейдами (например, скриншоты).
///     Предоставляет методы загрузки и управления файловыми ресурсами.
/// </summary>
public interface IRaidFileService
{
    /// <summary>
    ///     Загружает файл рейда в хранилище и возвращает информацию о загруженном файле.
    /// </summary>
    /// <param name="request">Объект, содержащий содержимое файла, имя и MIME-тип.</param>
    /// <param name="raidId">Идентификатор рейда, к которому относится загружаемый файл.</param>
    /// <returns>Информация о загруженном файле в виде объекта <see cref="FileDto" />.</returns>
    Task<FileDto> UploadAsync(FileUploadRequest request, int raidId);
}