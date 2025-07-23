using Freaks.Files.Bll.Interfaces.RaidFiles;
using Freaks.Files.SharedContracts.ValueObjects;

namespace Freaks.Files.Bll.Interfaces;

/// <summary>
///     Фабрика для получения сервиса обработки файлов в зависимости от их типа.
///     Позволяет выбирать подходящую реализацию (например, для скриншотов, логов, вложений и т.д.).
/// </summary>
public interface IFileServiceFactory
{
    /// <summary>
    ///     Возвращает реализацию сервиса, соответствующую заданному типу файла.
    /// </summary>
    /// <param name="fileType">Тип файла (например, <see cref="RaidFileType.Screenshot" />).</param>
    /// <returns>Экземпляр сервиса, реализующего <see cref="IRaidFileService" /> или его специализацию.</returns>
    IRaidFileService GetRaidFileService(RaidFileType fileType);
}
