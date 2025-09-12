"use client";
import { useSession } from "next-auth/react";
import { useMemo } from "react";

// Маппинг строковых ролей Keycloak на числовые коды прав доступа
const ROLE_MAPPING: Record<string, number> = {
  'admin': 30,           // Администратор
  'guild_leader': 40,    // Гильд-лидер
  'member': 20,          // Участник
  'editor': 10,          // Редактор
  // Добавьте другие роли по необходимости
};

export const useHasNumericRole = (required: number | number[] = []) => {
  const { data: session, status } = useSession();
  const isLoading = status === "loading";

  const userRoles = useMemo(() => {
    // Получаем строковые роли из сессии
    const stringRoles = (session?.user as any)?.roles ?? [] as string[];
    
    // Преобразуем в числовые коды
    const numericRoles: number[] = [];
    stringRoles.forEach(role => {
      if (ROLE_MAPPING[role]) {
        numericRoles.push(ROLE_MAPPING[role]);
      }
    });
    
    return numericRoles;
  }, [session]);

  const hasAccess = useMemo(() => {
    if (isLoading || !userRoles.length) return false;

    if (Array.isArray(required)) {
      return required.some((r) => userRoles.includes(r));
    }

    return userRoles.includes(required);
  }, [userRoles, required, isLoading]);

  return {
    hasAccess,
    isLoading,
    userRoles,
  };
};
