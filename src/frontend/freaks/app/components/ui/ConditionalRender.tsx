"use client";

import React, { ReactNode } from "react";
import { useHasRole } from "@/domains/auth/hooks/useHasRole";
import type { RequiredRoles } from "@/types/roles.types";

type Props = {
  children: ReactNode;
  requiredRoles: RequiredRoles;
  fallback?: ReactNode;
  showIfNoAccess?: boolean;
};

/**
 * Компонент для условного рендеринга контента на основе ролей пользователя
 * Не выполняет редирект, просто показывает/скрывает контент
 */
const ConditionalRender = ({
  children,
  requiredRoles,
  fallback = null,
  showIfNoAccess = false,
}: Props) => {
  const { hasAccess, isLoading } = useHasRole(requiredRoles);

  if (isLoading) {
    return fallback;
  }

  if (showIfNoAccess) {
    return !hasAccess ? <>{children}</> : fallback;
  }

  return hasAccess ? <>{children}</> : fallback;
};

export default ConditionalRender;
