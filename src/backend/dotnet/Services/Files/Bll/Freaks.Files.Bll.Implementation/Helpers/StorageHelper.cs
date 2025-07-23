namespace Freaks.Files.Bll.Implementation.Helpers;

/// <summary>
///     Вспомогательные методы для работы с объектным хранилищем (MinIO, S3 и т.п.).
///     Содержит генераторы имён файлов и методы построения URL/URI.
/// </summary>
public static class StorageHelper
{
    /// <summary>
    ///     Генерирует уникальное имя файла на основе GUID и сохраняет оригинальное расширение.
    /// </summary>
    /// <param name="originalFileName">Оригинальное имя файла (например, <c>photo.jpg</c>).</param>
    /// <returns>Новое имя файла, например: <c>f6a7b0c3d19e4899b4a9fa06d44c3b2a.jpg</c>.</returns>
    public static string GenerateFileName(string originalFileName)
    {
        return $"{Guid.NewGuid():N}{Path.GetExtension(originalFileName)}";
    }

    /// <summary>
    ///     Формирует абсолютный URL к файлу на основе хоста, bucket'а, пути и имени файла.
    /// </summary>
    /// <param name="host">Базовый адрес хранилища (например, <c>https://storage.example.com</c>).</param>
    /// <param name="bucketName">Наименование корзины (Bucket).</param>
    /// <param name="filePath">Виртуальный путь (папка) внутри хранилища (например, <c>raids/1/screenshots</c>).</param>
    /// <param name="fileName">Имя файла, включая расширение (например, <c>image.png</c>).</param>
    /// <returns>Абсолютный URL к файлу.</returns>
    public static string GetFileUrl(string host, string bucketName, string filePath, string fileName)
    {
        return $"{host.TrimEnd('/')}/{bucketName}/{filePath.TrimEnd('/')}/{fileName}";
    }

    /// <summary>
    ///     Формирует абсолютный URL к файлу на основе хоста, bucket'а, пути и имени файла.
    /// </summary>
    /// <param name="scheme">Схема адреса хранилища (например, <c>http</c>)</param>
    /// <param name="host">Базовый адрес хранилища (например, <c>https://storage.example.com</c>).</param>
    /// <param name="bucketName">Наименование корзины (Bucket).</param>
    /// <param name="filePath">Виртуальный путь (папка) внутри хранилища (например, <c>raids/1/screenshots</c>).</param>
    /// <param name="fileName">Имя файла, включая расширение (например, <c>image.png</c>).</param>
    /// <returns>Абсолютный URL к файлу.</returns>
    public static string GetFileUrl(string scheme, string host, string bucketName, string filePath, string fileName)
    {
        return $"{scheme}://{host.TrimEnd('/')}/{bucketName}/{filePath.TrimEnd('/')}/{fileName}";
    }

    /// <summary>
    ///     Формирует относительный путь (URI) к файлу, который может использоваться для хранения или логики доступа.
    /// </summary>
    /// <param name="bucketName">Наименование корзины (Bucket).</param>
    /// <param name="filePath">Путь внутри хранилища.</param>
    /// <param name="fileName">Имя файла.</param>
    /// <returns>Относительный URI к файлу, например: <c>raids/1/screenshots/image.png</c>.</returns>
    public static string GetFileUri(string bucketName, string filePath, string fileName)
    {
        return $"{bucketName}/{filePath.TrimEnd('/')}/{fileName}";
    }
}