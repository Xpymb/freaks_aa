namespace Freaks.Users.Contracts.ValueObjects;

/// <summary>
///     Контекст текущего пользователя, содержащий основные идентификационные и пользовательские данные.
///     Используется для получения информации о пользователе внутри приложения без обращения к БД.
/// </summary>
public interface IUserContext
{
    /// <summary>
    ///     Уникальный идентификатор пользователя.
    /// </summary>
    Guid Id { get; init; }

    /// <summary>
    ///     Список ролей пользователя
    /// </summary>
    List<UserRole> Roles { get; init; }

    /// <summary>
    ///     Username пользователя (уникальное имя для входа или отображения).
    /// </summary>
    string Username { get; init; }

    /// <summary>
    ///     Электронная почта пользователя.
    /// </summary>
    string Email { get; init; }

    /// <summary>
    ///     Игровой никнейм пользователя.
    /// </summary>
    string GameNickname { get; init; }

    /// <summary>
    ///     Дата и время создания пользователя.
    /// </summary>
    DateTimeOffset CreatedDt { get; init; }

    /// <summary>
    ///     Дата и время последнего обновления информации о пользователе.
    /// </summary>
    DateTimeOffset UpdatedDt { get; init; }
}