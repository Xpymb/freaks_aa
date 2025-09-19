"use client";

import { useMemo } from "react";
import { useAuth } from "@/store/authTokenStore";
import { RaidItem } from "../types";
import { getRaidPermissions } from "../permissions";
import { mapUserRoles } from "@/utils/roleUtils";

/**
 * Оптимизированный хук для получения прав доступа к рейду с мемоизацией
 */
export function useRaidPermissions(raid: RaidItem) {
  const { user, isAuthenticated } = useAuth();

  return useMemo(() => {
    const rawRoles = user?.roles || [];
    const userRoles = mapUserRoles(rawRoles);
    const userId = user?.id;
    const permissions = getRaidPermissions(userRoles, raid, userId);

    return {
      ...permissions,
      userRoles,
      userId,
      isAuthenticated,
    };
  }, [user, raid, isAuthenticated]);
}
