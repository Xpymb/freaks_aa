"use client";
import { useSession } from "next-auth/react";

/**
 * Простой хук для проверки ролей пользователя
 * @param requiredRoles - массив строковых ролей, которые нужно проверить
 * @returns объект с информацией о доступе
 */
export const useHasRole = (requiredRoles: string | string[]) => {
  const { data: session, status } = useSession();
  
  // Нормализуем входные данные в массив
  const rolesArray = Array.isArray(requiredRoles) ? requiredRoles : [requiredRoles];
  
  // Получаем роли пользователя из сессии
  const userRoles = session?.user?.roles || [];
  
  // Проверяем, есть ли у пользователя хотя бы одна из требуемых ролей
  const hasAccess = rolesArray.some(role => userRoles.includes(role));
  
  return {
    hasAccess,
    isLoading: status === "loading",
    userRoles,
    session
  };
};
