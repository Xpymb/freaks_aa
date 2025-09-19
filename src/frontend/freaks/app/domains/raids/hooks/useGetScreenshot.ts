"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { RaidScreenshotsService } from "@/domains/raids/raids.service";
import type { IRaidScreenshot } from "@/domains/raids/types";
import type {
  SSEMessage,
  RaidScreenshotChangedMessage,
} from "@/types/sse.types";
import type { Key } from "swr";

export const screenshotsKey = (raidId: number) =>
  `/raids/${raidId}/screenshots`;

export function useGetRaidScreenshots(
  prefetchData: IRaidScreenshot[],
  raidId: number
) {
  const key = typeof raidId === "number" ? screenshotsKey(raidId) : null;

  const { data, isLoading, errorState, mutate } = useProtectedSWR<
    IRaidScreenshot[]
  >(key, (token) => RaidScreenshotsService.getScreenshots(token, raidId), {
    fallbackData: prefetchData,
    revalidateOnMount: false,
    websocket: {
      channel: "raid.screenshot",
      enabled: true,
      onMessage: (data: SSEMessage, key: Key, mutate: () => void) => {
        const screenshotData = data?.pub?.data as RaidScreenshotChangedMessage;

        // Обновляем только если это изменение скриншотов для текущего рейда
        if (screenshotData?.RaidId === raidId) {
          mutate();
        }
      },
    },
  });

  return {
    screenshots: data ?? [],
    isLoading,
    errorState,
    refresh: () => mutate(),
  } as const;
}
