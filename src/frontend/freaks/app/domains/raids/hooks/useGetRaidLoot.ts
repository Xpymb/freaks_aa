"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { RaidLootService } from "../raids.service";
import type { RaidLootDto } from "../types";

export const raidLootKey = (raidId: number) =>
  `/raids/${raidId}/loots`;

export function useGetRaidLoot(
  prefetchData: RaidLootDto[],
  raidId: number
) {
  const key = typeof raidId === "number" ? raidLootKey(raidId) : null;

  const { data, isLoading, errorState, mutate } = useProtectedSWR<
    RaidLootDto[]
  >(key, (token) => RaidLootService.getRaidLoot(token, raidId), {
    fallbackData: prefetchData,
    revalidateOnMount: false,
  });

  return {
    lootItems: data ?? [],
    isLoading,
    errorState,
    refresh: () => mutate(),
  } as const;
}
