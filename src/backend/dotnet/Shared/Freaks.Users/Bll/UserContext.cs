namespace Freaks.Users.Bll;

/// <inheritdoc />
public class UserContext : IUserContext
{
    /// <inheritdoc />
    public required Guid Id { get; init; }

    /// <inheritdoc />
    public required string Username { get; init; }

    /// <inheritdoc />
    public required string Email { get; init; }

    /// <inheritdoc />
    public required string GameNickname { get; init; }

    /// <inheritdoc />
    public required DateTimeOffset CreatedDt { get; init; }

    /// <inheritdoc />
    public required DateTimeOffset UpdatedDt { get; init; }
}