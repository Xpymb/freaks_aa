namespace Freaks.Users.Contracts;

/// <summary>
///     Перечисление ролей пользователя с возможностью побитового сочетания.
/// </summary>
public enum UserRole
{
    /// <summary>
    ///     Обычный участник (базовая роль).
    /// </summary>
    Member = 10,

    /// <summary>
    ///     Редактор с правами на изменение контента.
    /// </summary>
    Editor = 20,

    /// <summary>
    ///     Администратор с расширенными правами доступа.
    /// </summary>
    Admin = 30,

    /// <summary>
    ///     Лидер гильдии — специальная роль, связанная с управлением гильдией.
    /// </summary>
    GuildLeader = 40,
}