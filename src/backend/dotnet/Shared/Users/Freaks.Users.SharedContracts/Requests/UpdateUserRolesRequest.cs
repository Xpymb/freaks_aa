using Freaks.Users.Contracts.ValueObjects;

namespace Freaks.Users.SharedContracts.Requests;

/// <summary>
///     Запрос на обновление ролей пользователя.
/// </summary>
/// <param name="UserRoles">Список новых ролей, которые необходимо назначить пользователю.</param>
public record UpdateUserRolesRequest(List<UserRole> UserRoles);