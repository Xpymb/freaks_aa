using Freaks.Messages.SharedContracts.ValueObjects;

namespace Freaks.Messages.SharedContracts.Messages;

/// <summary>
///     Базовый класс для сообщений в системе обмена данными,
///     содержащий информацию о типе действия и передаваемой полезной нагрузке.
/// </summary>
public abstract class BaseMessage
{
    /// <summary>
    ///     Тип действия, которое было выполнено над сущностью (создание, обновление, удаление).
    /// </summary>
    public required EntityActionType ActionType { get; init; }

    /// <summary>
    ///     Полезная нагрузка сообщения — данные, связанные с указанным действием.
    /// </summary>
    public object? Payload { get; init; }
}