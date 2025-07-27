namespace Freaks.Messages.SharedContracts.ValueObjects;

/// <summary>
///     Перечисление типов действий, выполняемых над сущностью.
/// </summary>
public enum EntityActionType
{
    /// <summary>
    ///     Сущность создана.
    /// </summary>
    Created = 1,

    /// <summary>
    ///     Сущность обновлена.
    /// </summary>
    Updated = 2,

    /// <summary>
    ///     Сущность удалена.
    /// </summary>
    Deleted = 3,
}