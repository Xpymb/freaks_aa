"use client";

import React, { ReactNode, memo } from "react";
import { useRaidPermissions } from "@/domains/raids";
import type { RaidItem } from "@/domains/raids/types";

interface RaidConditionalRenderProps {
  children: ReactNode;
  raid: RaidItem;
  permission:
    | "canView"
    | "canEdit"
    | "canEditInfo"
    | "canComplete"
    | "canDelete"
    | "canManage";
  fallback?: ReactNode;
}

/**
 * Компонент для условного рендеринга на основе прав доступа к рейду
 */
const RaidConditionalRender = memo(
  ({
    children,
    raid,
    permission,
    fallback = null,
  }: RaidConditionalRenderProps) => {
    const permissions = useRaidPermissions(raid);

    const hasAccess =
      permission === "canManage"
        ? permissions.canComplete || permissions.canDelete
        : permissions[permission];

    return hasAccess ? <>{children}</> : <>{fallback}</>;
  }
);

RaidConditionalRender.displayName = "RaidConditionalRender";

export default RaidConditionalRender;
