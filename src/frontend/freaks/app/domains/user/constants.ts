export const USER_ROLES = {
  ADMIN: 30,
  GUILD_LEADER: 40,
  EDITOR: 20,
  MEMBER: 10,
} as const;

export const ROLE_LABELS: Record<number, string> = {
  [USER_ROLES.ADMIN]: "Администратор",
  [USER_ROLES.GUILD_LEADER]: "Лидер гильдии",
  [USER_ROLES.EDITOR]: "Редактор",
  [USER_ROLES.MEMBER]: "Участник",
};

export const ROLE_COLORS: Record<
  number,
  "default" | "primary" | "secondary" | "error" | "info" | "success" | "warning"
> = {
  [USER_ROLES.ADMIN]: "error",
  [USER_ROLES.GUILD_LEADER]: "warning",
  [USER_ROLES.EDITOR]: "info",
  [USER_ROLES.MEMBER]: "default",
};

export type UserRoleType = (typeof USER_ROLES)[keyof typeof USER_ROLES];
