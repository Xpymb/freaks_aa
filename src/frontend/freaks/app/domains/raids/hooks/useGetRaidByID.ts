"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { RaidsService } from "../raids.service";
import type { RaidItem } from "../types";
import type { SSEMessage, RaidChangedMessage } from "@/types/sse.types";
import type { Key } from "swr";

export const raidKey = (raidId: number) => `/raids/${raidId}`;

export function useGetRaidByID(fallbackData: RaidItem, raidId: number) {
  const key = typeof raidId === "number" ? raidKey(raidId) : null;

  const { data, isLoading, errorState, mutate } = useProtectedSWR<RaidItem>(
    key,
    (token) => RaidsService.getRaidByID(token, raidId),
    {
      fallbackData,
      revalidateOnMount: false,
      websocket: {
        channel: "raid",
        enabled: true,
        onMessage: (data: SSEMessage, key: Key, mutate: () => void) => {
          const raidData = data?.pub?.data as RaidChangedMessage;

          if (!raidData?.Id) return;

          // Извлекаем ID из ключа хука (например, "/raids/123" -> "123")
          const keyStr = String(key);
          const keyRaidId = keyStr.match(/\/raids\/(\d+)/)?.[1];

          // Обновляем только если ID совпадает
          if (keyRaidId === String(raidData.Id)) {
            mutate();
          }
        },
      },
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
