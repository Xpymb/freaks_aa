"use client";

import { useState } from "react";
import { mutate } from "swr";
import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { FileService } from "@/domains/files/files.service";
import { RaidScreenshotsService } from "@/domains/raids/raids.service";
import type { IFileUploadResponse } from "@/domains/files/types";
import type { IRaidScreenshot } from "@/domains/raids/types";

export type UploadRaidScreenshotArgs = {
  raidId: number;
  fileType: number;
  file: File;
  onProgress?: (p: number) => void;
  optimistic?: boolean;
};

const screenshotsKey = (raidId: number) => `/raids/${raidId}/screenshots`;

export function useUploadRaidScreenshot() {
  const [progress, setProgress] = useState(0);

  const mutation = useProtectedSWRMutation<
    IRaidScreenshot[],
    UploadRaidScreenshotArgs
  >(
    "upload-raid-screenshot",
    async (
      token,
      { raidId, fileType, file, onProgress, optimistic = true }
    ) => {
      const key = screenshotsKey(raidId);

      // 1) upload в MinIO (с прогрессом)
      const uploaded: IFileUploadResponse = await FileService.postFile(
        token,
        { raidId, fileType, file },
        (p) => {
          setProgress(p);
          onProgress?.(p);
        }
      );

      // 2) optimistic update
      if (optimistic) {
        const ghost: IRaidScreenshot = {
          raidId,
          screenshotUri: uploaded.fileUri, // имя поля из твоего ответа
        } as IRaidScreenshot;

        mutate(key, (prev: IRaidScreenshot[] = []) => [...prev, ghost], false);
      }

      // 3) привязка к рейду
      await RaidScreenshotsService.postScreenshot(token, raidId, {
        screenshotUris: [uploaded.fileUri],
      });

      // 4) актуализируем список (твой сервис возвращает уже data)
      const refreshed = await RaidScreenshotsService.getScreenshots(
        token,
        raidId
      );
      mutate(key, refreshed, false);

      setProgress(0);
      return refreshed;
    }
  );

  return { ...mutation, progress } as const;
}
