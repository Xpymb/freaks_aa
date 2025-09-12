// useDeleteRaidScreenshot.ts
"use client";

import { mutate } from "swr";
import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { RaidScreenshotsService } from "@/domains/raids/raids.service";

export const screenshotsKey = (raidId: number) =>
  `/raids/${raidId}/screenshots`;

type DeleteByUrlArgs = {
  raidId: number;
  url: string;
};

export function useDeleteRaidScreenshot() {
  return useProtectedSWRMutation<void, DeleteByUrlArgs>(
    "delete-raid-screenshot",
    async (token, { raidId, url }) => {
      await RaidScreenshotsService.deleteScreenshotByUrl(token, raidId, url);
      await mutate(screenshotsKey(raidId));
    }
  );
}
