namespace Freaks.Files.Dal.Interfaces;

/// <summary>
///     Интерфейс для работы с объектным хранилищем (например, MinIO или S3).
///     Позволяет загружать, получать и удалять файлы по указанному пути и имени,
///     с возможностью указания кастомного bucket'а или использования значения по умолчанию.
/// </summary>
public interface IStorageProvider
{
    /// <summary>
    ///     Получает файл из хранилища как поток по его абсолютному пути.
    /// </summary>
    /// <param name="absolutePath">Полный путь к объекту внутри bucket'а, например: <c>raids/1/screenshots/image.png</c>.</param>
    /// <param name="bucket">Имя bucket'а (если <c>null</c>, используется значение по умолчанию из конфигурации).</param>
    /// <returns>Поток с содержимым файла.</returns>
    Task<Stream> GetAsync(string absolutePath, string? bucket = null);

    /// <summary>
    ///     Загружает файл в хранилище по указанному пути и имени.
    /// </summary>
    /// <param name="fileStream">Поток файла для загрузки.</param>
    /// <param name="path">Виртуальный путь (папка) внутри bucket'а, например: <c>raids/1/screenshots</c>.</param>
    /// <param name="fileName">Имя файла (например, <c>image.png</c>).</param>
    /// <param name="contentType">MIME-тип содержимого (например, <c>image/png</c>).</param>
    /// <param name="bucket">Имя bucket'а (если <c>null</c>, используется значение по умолчанию из конфигурации).</param>
    Task UploadAsync(Stream fileStream, string path, string fileName, string contentType, string? bucket = null);

    /// <summary>
    ///     Удаляет файл из хранилища по его абсолютному пути.
    /// </summary>
    /// <param name="absolutePath">Полный путь к объекту внутри bucket'а, например: <c>raids/1/screenshots/image.png</c>.</param>
    /// <param name="bucket">Имя bucket'а (если <c>null</c>, используется значение по умолчанию из конфигурации).</param>
    Task DeleteAsync(string absolutePath, string? bucket = null);
}

