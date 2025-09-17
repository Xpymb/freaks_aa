"use client";

import React, { useState, useEffect } from "react";
import { RaidItem, RaidParticipantDto, IRaidScreenshot } from "@/domains/raids";
import { useGetRaidParticipants } from "@/domains/raids/hooks/useGetRaidParticipants";
import { useGetRaidScreenshots } from "@/domains/raids/hooks/useGetScreenshot";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";
import ParticipantGrid from "./ParticipantGrid";
import FloatingScreenshotButton from "@/components/ui/FloatingScreenshotButton/FloatingScreenshotButton";
import styles from "./_styles.module.scss";
import { IUser } from "@/domains/user/types";

type Props = {
  raid: RaidItem;
  prefetchParticipants: RaidParticipantDto[];
  prefetchUsers: IUser[];
  prefetchScreenshots?: IRaidScreenshot[];
};

const Participants = ({
  raid,
  prefetchParticipants,
  prefetchUsers,
  prefetchScreenshots = [],
}: Props) => {
  const [refreshKey, setRefreshKey] = useState(0);

  const {
    data: participants = prefetchParticipants,
    isLoading,
    errorState,
    mutate,
  } = useGetRaidParticipants(prefetchParticipants, raid.id);

  // Получаем скриншоты для плавающей кнопки
  const { screenshots } = useGetRaidScreenshots(prefetchScreenshots, raid.id);

  // Обработчик изменения участников
  const handleParticipantsChange = () => {
    setRefreshKey((prev) => prev + 1);
    // Перезагружаем данные участников
    mutate();
  };

  // Слушаем события обновления участников
  useEffect(() => {
    const handleRefresh = () => {
      mutate();
    };

    window.addEventListener("refresh-participants", handleRefresh);
    return () => {
      window.removeEventListener("refresh-participants", handleRefresh);
    };
  }, [mutate]);

  if (isLoading) {
    return <DefaultLoader />;
  }

  if (errorState.isError) {
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
