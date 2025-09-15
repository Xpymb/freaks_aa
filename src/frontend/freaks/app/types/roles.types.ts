// Определяем все доступные роли в системе
export const AVAILABLE_ROLES = [
  "admin",
  "guild_leader",
  "editor",
  "member",
] as const;

// Тип для одной роли
export type UserRole = (typeof AVAILABLE_ROLES)[number];

// Тип для требуемых ролей (одна роль или массив ролей)
export type RequiredRoles = UserRole | UserRole[];

// Помощник для проверки, является ли строка валидной ролью
export function isValidRole(role: string): role is UserRole {
  return AVAILABLE_ROLES.includes(role as UserRole);
}

// Помощник для нормализации ролей в массив
export function normalizeRoles(roles: RequiredRoles): UserRole[] {
  return Array.isArray(roles) ? roles : [roles];
}
