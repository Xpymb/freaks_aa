"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { LootService } from "../loot.service";
import type { LootItemDto } from "../types";

export function useGetLootItems() {
  const { data, isLoading, errorState, mutate } = useProtectedSWR<
    LootItemDto[]
  >("/loot-items", (token) => LootService.getLootItems(token));

  return {
    lootItems: data ?? [],
    isLoading,
    errorState,
    refresh: () => mutate(),
  } as const;
}
