namespace Freaks.Users.Contracts;

/// <summary>
///     DTO (Data Transfer Object), представляющий краткую информацию о пользователе для передачи между слоями приложения.
/// </summary>
public class UserDto
{
    /// <summary>
    ///     Уникальный идентификатор пользователя.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Username пользователя.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    ///     Игровой никнейм пользователя.
    /// </summary>
    public required string GameNickname { get; init; }
}