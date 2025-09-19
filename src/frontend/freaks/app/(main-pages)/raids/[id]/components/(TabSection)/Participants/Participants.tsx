"use client";

import React, { useState, useEffect } from "react";
import { RaidItem, RaidParticipantDto, IRaidScreenshot } from "@/domains/raids";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";
import ParticipantGrid from "./ParticipantGrid";
import FloatingScreenshotButton from "@/components/ui/FloatingScreenshotButton/FloatingScreenshotButton";
import styles from "./_styles.module.scss";
import { IUser } from "@/domains/user/types";
import { useAppError } from "@/shared/errors";

type Props = {
  raid: RaidItem;
  participants: RaidParticipantDto[];
  participantsLoading: boolean;
  participantsError: ReturnType<typeof useAppError>;
  prefetchUsers: IUser[];
  screenshots: IRaidScreenshot[];
  onParticipantsChange?: () => void;
};

const Participants = ({
  raid,
  participants,
  participantsLoading,
  participantsError,
  prefetchUsers,
  screenshots,
  onParticipantsChange,
}: Props) => {
  const [refreshKey, setRefreshKey] = useState(0);

  // Обработчик изменения участников
  const handleParticipantsChange = () => {
    setRefreshKey((prev) => prev + 1);
    // Уведомляем родительский компонент об изменении
    onParticipantsChange?.();
  };

  if (participantsLoading) {
    return <DefaultLoader />;
  }

  if (participantsError?.isError) {
    return <ErrorLoadData />;
  }

  // Преобразуем скриншоты для плавающей кнопки
  const screenshotsForButton = screenshots.map((screenshot, index) => ({
    id: index.toString(),
    url: `${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${screenshot.screenshotUri}`,
    thumbnail: `${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${screenshot.screenshotUri}`,
  }));

  return (
    <div className={styles.participantsContainer}>
      <ParticipantGrid
        key={refreshKey}
        raidId={raid.id}
        raid={raid}
        participants={participants}
        prefetchUsers={prefetchUsers}
        onParticipantsChange={handleParticipantsChange}
      />

      {/* Плавающая кнопка со скриншотами */}
      <FloatingScreenshotButton
        screenshots={screenshotsForButton}
        isVisible={screenshots.length > 0}
      />
    </div>
  );
};

export default Participants;
