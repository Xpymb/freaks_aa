using Freaks.Users.Contracts.ValueObjects;

namespace Freaks.Users.SharedContracts.Dto;

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

    /// <summary>
    ///     Список ролей пользователя, представленных как элементы перечисления <see cref="UserRole" />.
    /// </summary>
    public required List<UserRole> Roles { get; init; }
}