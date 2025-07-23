using Freaks.Files.Bll.Implementation.RaidFiles;
using Freaks.Files.Bll.Interfaces;
using Freaks.Files.Bll.Interfaces.RaidFiles;
using Freaks.Files.SharedContracts.ValueObjects;

namespace Freaks.Files.Bll.Implementation;

/// <summary>
///     Реализация фабрики <see cref="IFileServiceFactory" />, которая предоставляет сервис обработки файлов на основе их
///     типа.
///     Использует список зарегистрированных реализаций для выбора подходящего сервиса.
/// </summary>
public class FileServiceFactory : IFileServiceFactory
{
    private readonly IEnumerable<IRaidFileService> _fileServiceList;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="FileServiceFactory" />.
    /// </summary>
    /// <param name="fileServiceList">Список доступных реализаций сервисов обработки файлов.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если список сервисов равен <c>null</c>.</exception>
    public FileServiceFactory(IEnumerable<IRaidFileService> fileServiceList)
    {
        _fileServiceList = fileServiceList ?? throw new ArgumentNullException(nameof(fileServiceList));
    }

    /// <summary>
    ///     Возвращает реализацию сервиса, соответствующую заданному типу файла.
    /// </summary>
    /// <param name="fileType">Тип файла, для которого требуется получить сервис.</param>
    /// <returns>Экземпляр <see cref="IRaidFileService" />, соответствующий типу файла.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если передан неподдерживаемый тип файла.</exception>
    public IRaidFileService GetRaidFileService(RaidFileType fileType)
    {
        return fileType switch
        {
            RaidFileType.Screenshot => _fileServiceList.First(s => s.GetType() == typeof(RaidScreenshotFileService)),
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null),
        };
    }
}
