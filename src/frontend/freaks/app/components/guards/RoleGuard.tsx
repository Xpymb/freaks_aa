"use client";

import React, { ReactNode } from "react";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import { useHasRole } from "@/domains/auth/hooks/useHasRole";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";
import type { RequiredRoles } from "@/types/roles.types";

type Props = {
  children: ReactNode;
  requiredRoles: RequiredRoles;
  redirectTo?: string;
  fallback?: ReactNode;
};

/**
 * Клиентский guard для проверки ролей пользователя
 * Редиректит на 403 если у пользователя нет необходимых ролей
 */
const RoleGuard = ({
  children,
  requiredRoles,
  redirectTo = "/forbidden",
  fallback,
}: Props) => {
  const router = useRouter();
  const { hasAccess, isLoading } = useHasRole(requiredRoles);

  useEffect(() => {
    if (!isLoading && !hasAccess) {
      router.push(redirectTo);
    }
  }, [isLoading, hasAccess, router, redirectTo]);

  if (isLoading) {
    return fallback || <DefaultLoader />;
  }

  if (!hasAccess) {
    return null; // Пока происходит редирект
  }

  return <>{children}</>;
};

export default RoleGuard;
