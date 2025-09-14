"use client";

import React from "react";
import {
  RaidItem,
  IRaidScreenshot,
  RaidLootDto,
  LootItemDto,
  RaidParticipantDto,
} from "@/domains/raids";
import { IUser } from "@/types/user.types";
import { useGetRaidByID } from "@/domains/raids/hooks/useGetRaidByID";
import HeaderBlock from "./HeaderBlock/HeaderBlock";
import BodyBlock from "./BodyBlock/BodyBlock";
import styles from "./_styles.module.scss";

type Props = {
  initialRaid: RaidItem;
  prefetchScreenshots: IRaidScreenshot[];
  prefetchLoot: RaidLootDto[];
  prefetchLootItems: LootItemDto[];
  prefetchParticipants: RaidParticipantDto[];
  prefetchUsers: IUser[];
};

const RaidPageClient = ({
  initialRaid,
  prefetchScreenshots,
  prefetchLoot,
  prefetchLootItems,
  prefetchParticipants,
  prefetchUsers,
}: Props) => {
  const { raid, mutate } = useGetRaidByID(initialRaid, initialRaid.id);

  const handleRaidUpdated = () => {
    mutate();
  };

  if (!raid) return null;

  return (
    <div className={styles.wrapper}>
      <HeaderBlock raid={raid} onRaidUpdated={handleRaidUpdated} />
      <BodyBlock
        raid={raid}
        prefetchScreenshots={prefetchScreenshots}
        prefetchLoot={prefetchLoot}
        prefetchLootItems={prefetchLootItems}
        prefetchParticipants={prefetchParticipants}
        prefetchUsers={prefetchUsers}
      />
    </div>
  );
};

export default RaidPageClient;
