"use client";
import { useAuth } from "@/store/authTokenStore";
import type { RequiredRoles, UserRole } from "@/types/roles.types";
import { normalizeRoles } from "@/types/roles.types";

/**
 * Простой хук для проверки ролей пользователя
 * @param requiredRoles - роль или массив ролей для проверки
 * @returns объект с информацией о доступе
 */
export const useHasRole = (requiredRoles: RequiredRoles) => {
  const { user, isAuthenticated } = useAuth();

  // Нормализуем входные данные в массив
  const rolesArray = normalizeRoles(requiredRoles);

  // Получаем роли пользователя из store
  const userRoles = (user?.roles || []) as UserRole[];

  // Проверяем, есть ли у пользователя хотя бы одна из требуемых ролей
  const hasAccess = rolesArray.some((role) => userRoles.includes(role));

  return {
    hasAccess,
    isLoading: !isAuthenticated && user === null,
    userRoles,
    user,
  };
};
