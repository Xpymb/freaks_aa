import type { UserRole } from "@/types/roles.types";
import { RaidItem, RaidStatus } from "./types";

// Роли с правами администрирования
const ADMIN_ROLES: UserRole[] = ["admin", "guild_leader", "editor"];

/**
 * Проверяет может ли пользователь редактировать содержимое рейда
 * (добавлять/удалять участников, лут, скриншоты и т.д.)
 */
export function canEditRaidContent(
  userRoles: UserRole[],
  raid: RaidItem,
  userId?: string
): boolean {
  // После завершения рейда никто не может редактировать
  if (raid.status === RaidStatus.Ended) {
    return false;
  }

  // Админы, лидеры гильдии и редакторы могут редактировать
  const hasAdminRole = ADMIN_ROLES.some((role) => userRoles.includes(role));
  if (hasAdminRole) {
    return true;
  }

  // Создатель рейда может редактировать свой рейд
  if (userId && raid.creator.id === userId) {
    return true;
  }

  return false;
}

/**
 * Проверяет может ли пользователь подтверждать рейд
 * Только админы, лидеры гильдии и редакторы
 * Создатель рейда НЕ может подтверждать свой рейд
 */
export function canCompleteRaid(
  userRoles: UserRole[],
  raid: RaidItem,
  userId?: string
): boolean {
  // После завершения рейда нельзя подтверждать
  if (raid.status === RaidStatus.Ended) {
    return false;
  }

  // Только админы, лидеры гильдии и редакторы
  const hasAdminRole = ADMIN_ROLES.some((role) => userRoles.includes(role));
  if (!hasAdminRole) {
    return false;
  }

  // Создатель рейда НЕ может подтверждать свой рейд
  if (userId && raid.creator.id === userId) {
    return false;
  }

  return true;
}

/**
 * Проверяет может ли пользователь удалять рейд
 */
export function canDeleteRaid(
  userRoles: UserRole[],
  raid: RaidItem,
  userId?: string
): boolean {
  // После завершения рейда удалять могут только админы
  if (raid.status === RaidStatus.Ended) {
    return userRoles.includes("admin");
  }

  // До завершения - админы, лидеры гильдии и редакторы или создатель
  return (
    ADMIN_ROLES.some((role) => userRoles.includes(role)) ||
    (userId ? raid.creator.id === userId : false)
  );
}

/**
 * Проверяет может ли пользователь создавать рейды
 */
export function canCreateRaid(userRoles: UserRole[]): boolean {
  // Создавать рейды могут все авторизованные пользователи
  return userRoles.length > 0;
}

/**
 * Проверяет может ли пользователь просматривать рейд
 */
export function canViewRaid(userRoles: UserRole[]): boolean {
  // Просматривать рейды могут все авторизованные пользователи
  return userRoles.length > 0;
}

/**
 * Получает список доступных действий для пользователя в контексте рейда
 */
export function getRaidPermissions(
  userRoles: UserRole[],
  raid: RaidItem,
  userId?: string
) {
  return {
    canView: canViewRaid(userRoles),
    canEdit: canEditRaidContent(userRoles, raid, userId),
    canComplete: canCompleteRaid(userRoles, raid, userId),
    canDelete: canDeleteRaid(userRoles, raid, userId),
    canCreate: canCreateRaid(userRoles),
    isCreator: userId ? raid.creator.id === userId : false,
    isCompleted: raid.status === RaidStatus.Ended,
  };
}
