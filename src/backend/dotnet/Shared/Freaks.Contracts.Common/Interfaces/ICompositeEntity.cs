namespace Freaks.Contracts.Common.Interfaces;

/// <summary>
///     Интерфейс для сущностей с составным (композитным) ключом.
///     Предоставляет метод для получения значения ключа, объединяющего несколько полей,
///     используемого при операциях поиска, обновления и удаления.
/// </summary>
/// <typeparam name="TKey">
///     Тип составного ключа, представляющий уникальную комбинацию полей сущности.
/// </typeparam>
public interface ICompositeEntity<out TKey>
{
    /// <summary>
    ///     Возвращает значение составного ключа.
    /// </summary>
    TKey GetCompositeKey();
}