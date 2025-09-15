"use client";

import { useMemo } from "react";
import { useSession } from "next-auth/react";
import { RaidItem } from "../types";
import { getRaidPermissions } from "../permissions";
import { mapUserRoles } from "@/utils/roleUtils";

/**
 * Оптимизированный хук для получения прав доступа к рейду с мемоизацией
 */
export function useRaidPermissions(raid: RaidItem) {
  const { data: session } = useSession();

  return useMemo(() => {
    const rawRoles = session?.user?.roles || [];
    const userRoles = mapUserRoles(rawRoles);
    const userId = session?.user?.id;
    const permissions = getRaidPermissions(userRoles, raid, userId);

    return {
      ...permissions,
      userRoles,
      userId,
      isAuthenticated: !!session,
    };
  }, [session, raid]);
}
