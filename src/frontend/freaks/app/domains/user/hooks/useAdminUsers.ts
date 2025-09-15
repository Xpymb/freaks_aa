"use client";

import { useCallback } from "react";
import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { UserService, UpdateUserRolesRequest } from "../user.service";
import type { IUser } from "@/types/user.types";

export function useAdminUsers() {
  const {
    data: users,
    isLoading,
    errorState,
    mutate,
  } = useProtectedSWR<IUser[]>("/users", (token) =>
    UserService.getAllUsers(token)
  );

  const refresh = useCallback(() => {
    mutate();
  }, [mutate]);

  return {
    users: users ?? [],
    isLoading,
    errorState,
    refresh,
  } as const;
}

export function useUpdateUserRoles() {
  return useProtectedSWRMutation<IUser, UpdateUserRolesRequest>(
    "update-user-roles",
    (token, data) => UserService.updateUserRoles(token, data),
    {
      onSuccess: () => {
        window.dispatchEvent(new CustomEvent("refresh-admin-users"));
      },
    }
  );
}
