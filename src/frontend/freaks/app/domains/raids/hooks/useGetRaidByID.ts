"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { RaidsService } from "../raids.service";
import type { RaidItem } from "../types";

export const raidKey = (raidId: number) => `/raids/${raidId}`;

export function useGetRaidByID(fallbackData: RaidItem, raidId: number) {
  const key = typeof raidId === "number" ? raidKey(raidId) : null;

  const { data, isLoading, errorState, mutate } = useProtectedSWR<RaidItem>(
    key,
    (token) => RaidsService.getRaidByID(token, raidId),
    {
      fallbackData,
      revalidateOnMount: false,
    }
  );

  return {
    raid: data,
    isLoading,
    errorState,
    refresh: () => mutate(),
    mutate,
  } as const;
}
