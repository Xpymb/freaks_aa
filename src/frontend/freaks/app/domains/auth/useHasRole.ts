"use client";
import { useProfile, UserRole } from "@/domains/user/profile";
import { useMemo } from "react";

export const useHasRole = (required: UserRole | UserRole[] = []) => {
  const { profile, isLoading } = useProfile();

  const userRoles = useMemo(() => profile?.roles ?? [], [profile?.roles]);

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
