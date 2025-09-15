import { USER_ROLES } from "@/domains/user/constants";
import type { UserRole } from "@/types/roles.types";

/**
 * Преобразует роли (числовые или строковые) в валидные роли приложения
 */
export function mapUserRoles(roles: unknown): UserRole[] {
  if (!Array.isArray(roles)) {
    return [];
  }

  const roleMap: Record<number, UserRole> = {
    [USER_ROLES.ADMIN]: "admin",
    [USER_ROLES.GUILD_LEADER]: "guild_leader",
    [USER_ROLES.EDITOR]: "editor",
    [USER_ROLES.MEMBER]: "member",
  };

  // Валидные роли приложения
  const validAppRoles: UserRole[] = [
    "admin",
    "guild_leader",
    "editor",
    "member",
  ];

  return roles
    .map((role) => {
      // Если роль уже строковая, проверяем что это валидная роль приложения
      if (typeof role === "string") {
        return validAppRoles.includes(role as UserRole)
          ? (role as UserRole)
          : undefined;
      }
      // Если роль числовая, конвертируем
      if (typeof role === "number") {
        return roleMap[role];
      }
      return undefined;
    })
    .filter((role): role is UserRole => role !== undefined);
}
