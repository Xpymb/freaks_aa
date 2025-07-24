using Freaks.Users.Contracts;

namespace Freaks.Users.SharedContracts;

public record UpdateUserRolesRequest(List<UserRole> UserRoles);