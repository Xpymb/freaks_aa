import type { UserRole } from "@/types/roles.types";
import { RaidItem, RaidStatus } from "./types";

// Роли с полными правами администрирования
const SUPER_ADMIN_ROLES: UserRole[] = ["admin", "guild_leader"];

// Роли с правами редактирования (используется в комментариях)
// const EDITOR_ROLES: UserRole[] = ["admin", "guild_leader", "editor"];

// Роли с базовыми правами
const MEMBER_ROLES: UserRole[] = ["admin", "guild_leader", "editor", "member"];

/**
 * Проверяет может ли пользователь редактировать содержимое рейда
 * (добавлять/удалять участников, лут, скриншоты и т.д.)
 */
export function canEditRaidContent(
  userRoles: UserRole[],
  raid: RaidItem,
  userId?: string
): boolean {
  // После завершения рейда больше никто не может редактировать
  if (raid.status === RaidStatus.Ended) {
    return false;
  }

  // ADMIN и GUILD_LEADER могут редактировать незавершенные рейды
  const hasSuperAdminRole = SUPER_ADMIN_ROLES.some((role) =>
    userRoles.includes(role)
  );
  if (hasSuperAdminRole) {
    return true;
  }

  // EDITOR может редактировать любые незавершенные рейды
  const hasEditorRole = userRoles.includes("editor");
  if (hasEditorRole) {
    return true;
  }

  // MEMBER может редактировать только свои незавершенные рейды
  const hasMemberRole = userRoles.includes("member");
  if (hasMemberRole && userId && raid.creator.id === userId) {
    return true;
  }

  return false;
}

/**
 * Проверяет может ли пользователь подтверждать/завершать рейд
 * Только администраторы и лидеры гильдии
 */
export function canCompleteRaid(
  userRoles: UserRole[],
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  _raid: RaidItem,
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  _userId?: string
): boolean {
  // Только ADMIN и GUILD_LEADER могут завершать рейды
  const hasSuperAdminRole = SUPER_ADMIN_ROLES.some((role) =>
    userRoles.includes(role)
  );
  if (!hasSuperAdminRole) {
    return false;
  }

  // Могут завершать в любом статусе (даже уже завершенные для переоткрытия)
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
  // ADMIN и GUILD_LEADER могут удалять рейды в любом статусе
  const hasSuperAdminRole = SUPER_ADMIN_ROLES.some((role) =>
    userRoles.includes(role)
  );
  if (hasSuperAdminRole) {
    return true;
  }

  // После завершения рейда больше никто не может удалять
  if (raid.status === RaidStatus.Ended) {
    return false;
  }

  // EDITOR и MEMBER могут удалять только свои незавершенные рейды
  const hasEditorOrMemberRole =
    userRoles.includes("editor") || userRoles.includes("member");
  if (hasEditorOrMemberRole && userId && raid.creator.id === userId) {
    return true;
  }

  return false;
}

/**
 * Проверяет может ли пользователь редактировать основные данные рейда
 * (название босса, формат, описание)
 */
export function canEditRaidInfo(
  userRoles: UserRole[],
  raid: RaidItem,
  userId?: string
): boolean {
  // После завершения рейда больше никто не может редактировать
  if (raid.status === RaidStatus.Ended) {
    return false;
  }

  // ADMIN и GUILD_LEADER могут редактировать незавершенные рейды
  const hasSuperAdminRole = SUPER_ADMIN_ROLES.some((role) =>
    userRoles.includes(role)
  );
  if (hasSuperAdminRole) {
    return true;
  }

  // EDITOR может редактировать любые незавершенные рейды
  const hasEditorRole = userRoles.includes("editor");
  if (hasEditorRole) {
    return true;
  }

  // MEMBER может редактировать только свои незавершенные рейды
  const hasMemberRole = userRoles.includes("member");
  if (hasMemberRole && userId && raid.creator.id === userId) {
    return true;
  }

  return false;
}

/**
 * Проверяет может ли пользователь создавать рейды
 */
export function canCreateRaid(userRoles: UserRole[]): boolean {
  // Создавать рейды могут все пользователи с валидными ролями
  return MEMBER_ROLES.some((role) => userRoles.includes(role));
}

/**
 * Проверяет может ли пользователь просматривать рейд
 */
export function canViewRaid(userRoles: UserRole[]): boolean {
  // Просматривать рейды могут все пользователи с валидными ролями
  return MEMBER_ROLES.some((role) => userRoles.includes(role));
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
    canEditInfo: canEditRaidInfo(userRoles, raid, userId),
    canComplete: canCompleteRaid(userRoles, raid, userId),
    canDelete: canDeleteRaid(userRoles, raid, userId),
    canCreate: canCreateRaid(userRoles),
    isCreator: userId ? raid.creator.id === userId : false,
    isCompleted: raid.status === RaidStatus.Ended,
  };
}
