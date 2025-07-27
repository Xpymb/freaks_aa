namespace Freaks.Users.Contracts.Attributes;

/// <summary>
///     Атрибут для задания имени роли пользователя.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class UserRoleNameAttribute : Attribute
{
    /// <summary>
    ///     Имя роли пользователя.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UserRoleNameAttribute" />,
    ///     задавая указанное имя роли.
    /// </summary>
    /// <param name="name">Имя роли пользователя.</param>
    public UserRoleNameAttribute(string name)
    {
        Name = name;
    }
}