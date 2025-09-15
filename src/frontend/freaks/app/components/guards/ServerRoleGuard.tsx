import { redirect } from "next/navigation";
import { ReactNode } from "react";
import { auth } from "@/api/auth/auth";
import type { RequiredRoles, UserRole } from "@/types/roles.types";
import { normalizeRoles } from "@/types/roles.types";

type Props = {
  children: ReactNode;
  requiredRoles: RequiredRoles;
};

/**
 * Серверный guard для проверки ролей пользователя
 * Редиректит на 403 если у пользователя нет необходимых ролей
 */
export async function ServerRoleGuard({ children, requiredRoles }: Props) {
  const session = await auth();

  if (!session) {
    redirect("/login");
  }

  const userRoles = (session.user?.roles || []) as UserRole[];
  const rolesArray = normalizeRoles(requiredRoles);
  const hasAccess = rolesArray.some((role) => userRoles.includes(role));

  if (!hasAccess) {
    redirect("/forbidden");
  }

  return <>{children}</>;
}
