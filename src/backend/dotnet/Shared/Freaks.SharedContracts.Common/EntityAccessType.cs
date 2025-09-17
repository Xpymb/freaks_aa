namespace Freaks.SharedContracts.Common;

/// <summary>
/// Типы доступа к сущности (CRUD-операции).
/// </summary>
public enum EntityAccessType
{
    /// <summary>
    /// Просмотр сущности (чтение без изменений).
    /// </summary>
    View = 1,

    /// <summary>
    /// Обновление существующей сущности (редактирование).
    /// </summary>
    Update = 2,

    /// <summary>
    /// Создание новой сущности.
    /// </summary>
    Create = 3,

    /// <summary>
    /// Удаление сущности (жёсткое или логическое).
    /// </summary>
    Delete = 4
}