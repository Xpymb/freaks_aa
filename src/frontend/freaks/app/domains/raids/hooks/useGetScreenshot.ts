"use client";

import { useProtectedSWR } from "@/shared/api/useProtectedSWR";
import { RaidScreenshotsService } from "@/domains/raids/raids.service";
import type { IRaidScreenshot } from "@/domains/raids/types";

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
  });

  return {
    screenshots: data ?? [],
    isLoading,
    errorState,
    refresh: () => mutate(),
  } as const;
}
