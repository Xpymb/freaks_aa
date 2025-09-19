"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { RaidLootService } from "../raids.service";
import type { RaidLootDto } from "../types";
import type { SSEMessage, RaidLootChangedMessage } from "@/types/sse.types";
import type { Key } from "swr";

export const raidLootKey = (raidId: number) => `/raids/${raidId}/loots`;

export function useGetRaidLoot(prefetchData: RaidLootDto[], raidId: number) {
  const key = typeof raidId === "number" ? raidLootKey(raidId) : null;

  const { data, isLoading, errorState, mutate } = useProtectedSWR<
    RaidLootDto[]
  >(key, (token) => RaidLootService.getRaidLoot(token, raidId), {
    fallbackData: prefetchData,
    revalidateOnMount: false,
    websocket: {
      channel: "raid.loot",
      enabled: true,
      onMessage: (data: SSEMessage, key: Key, mutate: () => void) => {
        const lootData = data?.pub?.data as RaidLootChangedMessage;

        // Обновляем только если это изменение лута для текущего рейда
        if (lootData?.RaidId === raidId) {
          mutate();
        }
      },
    },
  });

  return {
    lootItems: data ?? [],
    isLoading,
    errorState,
    refresh: () => mutate(),
  } as const;
}
