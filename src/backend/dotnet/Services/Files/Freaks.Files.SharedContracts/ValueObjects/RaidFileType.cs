namespace Freaks.Files.SharedContracts.ValueObjects;

/// <summary>
///     Типы файлов, связанных с рейдом.
///     Используется для категоризации загружаемых и хранимых файлов (например, скриншоты, реплеи и т.д.).
/// </summary>
public enum RaidFileType
{
    /// <summary>
    ///     Скриншот рейда (например, снимок экрана боевой сцены или результата).
    /// </summary>
    Screenshot = 1,
}