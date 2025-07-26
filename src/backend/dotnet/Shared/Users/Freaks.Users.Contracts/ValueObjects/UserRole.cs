using Freaks.Users.Contracts.Attributes;

namespace Freaks.Users.Contracts.ValueObjects;

/// <summary>
///     Перечисление ролей пользователя с возможностью побитового сочетания.
/// </summary>
public enum UserRole
{
    /// <summary>
    ///     Обычный участник (базовая роль).
    /// </summary>
    [UserRoleName("member")]
    Member = 10,

    /// <summary>
    ///     Редактор с правами на изменение контента.
    /// </summary>
    [UserRoleName("editor")]
    Editor = 20,

    /// <summary>
    ///     Администратор с расширенными правами доступа.
    /// </summary>
    [UserRoleName("admin")]
    Admin = 30,

    /// <summary>
    ///     Лидер гильдии — специальная роль, связанная с управлением гильдией.
    /// </summary>
    [UserRoleName("guild_leader")]
    GuildLeader = 40,
}