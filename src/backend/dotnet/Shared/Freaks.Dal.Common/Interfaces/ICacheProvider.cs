namespace Freaks.Dal.Common.Interfaces;

/// <summary>
///     Интерфейс для работы с механизмом кэширования.
/// </summary>
public interface ICacheProvider
{
    /// <summary>
    ///     Асинхронно получает значение типа <c>T</c> из кэша по указанному ключу.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ кэша.</param>
    /// <returns>Значение из кэша или null, если не найдено.</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    ///     Асинхронно сохраняет значение типа <c>T</c> в кэш с заданным временем жизни.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ кэша.</param>
    /// <param name="value">Значение для сохранения.</param>
    /// <param name="expiration">Время хранения значения в кэше.</param>
    Task SetAsync<T>(string key, T value, TimeSpan expiration);

    /// <summary>
    ///     Асинхронно удаляет значение из кэша по указанному ключу.
    /// </summary>
    /// <param name="key">Ключ кэша для удаления.</param>
    Task RemoveAsync(string key);

    /// <summary>
    ///     Асинхронно удаляет значение из кэша по списку ключей.
    /// </summary>
    /// <param name="keys">Список ключей кэша для удаления.</param>
    Task RemoveAsync(List<string> keys);

    /// <summary>
    ///     Асинхронно удаляет все значения из кэша, ключи которых начинаются с указанного префикса.
    /// </summary>
    /// <param name="prefix">Префикс ключей для удаления.</param>
    Task RemoveByPrefixAsync(string prefix);

    /// <summary>
    ///     Асинхронно удаляет все значения из кэша, ключи которых начинаются с указанных префиксов.
    /// </summary>
    /// <param name="prefixes">Список префиксов ключей для удаления.</param>
    Task RemoveByPrefixAsync(List<string> prefixes);
}