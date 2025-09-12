"use client";

import React, { useState, useEffect } from "react";
import { RaidItem, RaidParticipantDto } from "@/domains/raids";
import { IUser } from "@/types/user.types";
import { useGetRaidParticipants } from "@/domains/raids/hooks/useGetRaidParticipants";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";
import ParticipantGrid from "./ParticipantGrid";
import styles from "./_styles.module.scss";

type Props = {
  raid: RaidItem;
  prefetchParticipants: RaidParticipantDto[];
  prefetchUsers: IUser[];
};

const Participants = ({ raid, prefetchParticipants, prefetchUsers }: Props) => {
  const [refreshKey, setRefreshKey] = useState(0);
  
  const { 
    data: participants = prefetchParticipants, 
    isLoading, 
    errorState,
    mutate
  } = useGetRaidParticipants(prefetchParticipants, raid.id);

  // Обработчик изменения участников
  const handleParticipantsChange = () => {
    setRefreshKey(prev => prev + 1);
    // Перезагружаем данные участников
    mutate();
  };

  // Слушаем события обновления участников
  useEffect(() => {
    const handleRefresh = () => {
      mutate();
    };

    window.addEventListener('refresh-participants', handleRefresh);
    return () => {
      window.removeEventListener('refresh-participants', handleRefresh);
    };
  }, [mutate]);

  if (isLoading) {
    return <DefaultLoader />;
  }

  if (errorState.isError) {
    return <ErrorLoadData />;
  }

  return (
    <div className={styles.participantsContainer}>
      <ParticipantGrid
        key={refreshKey}
        raidId={raid.id}
        participants={participants}
        prefetchUsers={prefetchUsers}
        onParticipantsChange={handleParticipantsChange}
      />
    </div>
  );
};

export default Participants;
