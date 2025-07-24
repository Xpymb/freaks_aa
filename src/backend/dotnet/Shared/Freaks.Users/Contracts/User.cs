using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;

namespace Freaks.Users.Contracts;

/// <summary>
///     Представляет пользователя в системе.
/// </summary>
[Table("users", Schema = DatabaseConsts.UsersSchema)]
public class User : IEntity<Guid>
{
    /// <summary>
    ///     Уникальный идентификатор пользователя.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; init; }

    /// <summary>
    ///     Роли пользователя
    /// </summary>
    [Column("roles")]
    public List<UserRole> Roles { get; set; } = [];

    /// <summary>
    ///     Уникальное имя пользователя (username), используемое для входа или отображения.
    /// </summary>
    [Column("username")]
    public required string Username { get; init; }

    /// <summary>
    ///     Электронная почта пользователя.
    /// </summary>
    [Column("email")]
    public required string Email { get; init; }

    /// <summary>
    ///     Игровой никнейм пользователя, отображаемый в игровых разделах.
    /// </summary>
    [Column("game_nickname")]
    public required string GameNickname { get; init; }

    /// <summary>
    ///     Дата и время создания записи о пользователе.
    /// </summary>
    [Column("created_dt")]
    public required DateTimeOffset CreatedDt { get; init; }

    /// <summary>
    ///     Дата и время последнего обновления записи о пользователе.
    /// </summary>
    [Column("updated_dt")]
    public DateTimeOffset UpdatedDt { get; set; }
}