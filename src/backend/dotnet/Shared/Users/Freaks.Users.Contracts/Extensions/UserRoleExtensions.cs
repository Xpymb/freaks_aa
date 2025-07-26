using System.Reflection;
using Freaks.Users.Contracts.Attributes;
using Freaks.Users.Contracts.ValueObjects;

namespace Freaks.Users.Contracts.Extensions;

/// <summary>
///     Содержит методы-расширения для работы с перечислением <see cref="UserRole" />, включая получение строковых имён
///     ролей
///     из атрибута <see cref="UserRoleNameAttribute" /> и преобразование строк в значения перечисления.
/// </summary>
public static class UserRoleExtensions
{
    /// <summary>
    ///     Возвращает словарь всех значений перечисления <see cref="UserRole" /> и соответствующих им имён ролей из атрибута
    ///     <see cref="UserRoleNameAttribute" />.
    /// </summary>
    public static Dictionary<UserRole, string> GetRoleNames()
    {
        return Enum.GetValues<UserRole>()
                   .ToDictionary(
                       role => role,
                       role => role.GetRoleName()
                   );
    }

    /// <summary>
    ///     Возвращает строковое имя роли, заданное через <see cref="UserRoleNameAttribute" />. Если не найдено — возвращает
    ///     <see cref="Enum.ToString()" />.
    /// </summary>
    public static string GetRoleName(this UserRole role)
    {
        var memberInfo =
            typeof(UserRole).GetMember(role.ToString())
                            .FirstOrDefault();

        if (memberInfo == null)
        {
            return role.ToString();
        }

        var attr = memberInfo.GetCustomAttribute<UserRoleNameAttribute>();
        if (attr != null)
        {
            return attr.Name;
        }

        return role.ToString();
    }

    /// <summary>
    ///     Возвращает значение перечисления <see cref="UserRole" />, соответствующее заданному строковому имени роли,
    ///     определённому через атрибут <see cref="UserRoleNameAttribute" />.
    /// </summary>
    /// <param name="roleName">Имя роли, заданное в атрибуте <see cref="UserRoleNameAttribute" />.</param>
    /// <returns>Значение <see cref="UserRole" />, соответствующее переданному имени.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Если не найдено ни одного значения <see cref="UserRole" /> с заданным именем.
    /// </exception>
    public static UserRole? GetRoleType(string roleName)
    {
        var roleNames = GetRoleNames();

        return roleNames.FirstOrDefault(r => r.Value == roleName)
                        .Key;
    }
}