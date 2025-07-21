namespace Freaks.Contracts.Common.Interfaces;

/// <summary>
///     Базовый интерфейс для всех сущностей в доменной модели.
///     Предоставляет уникальный идентификатор.
/// </summary>
public interface IEntity<out TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     Уникальный идентификатор сущности.
    /// </summary>
    TKey Id { get; }
}