"use client";

import styles from "./_styles.module.scss";
import RaidTabs from "../RaidTabs/RaidTabs";
import { JSX, useMemo, useState } from "react";
import Overview from "../(TabSection)/Overview/Overview";
import {
  IRaidScreenshot,
  RaidItem,
  RaidLootDto,
  LootItemDto,
  RaidParticipantDto,
} from "@/domains/raids";
import { IUser } from "@/types/user.types";
import Screenshots from "../(TabSection)/Screenshots/Screenshots";
import Loot from "../(TabSection)/Loot/Loot";
import Participants from "../(TabSection)/Participants/Participants";
import { useGetRaidScreenshots } from "@/domains/raids/hooks/useGetScreenshot";

type Props = {
  raid: RaidItem;
  prefetchScreenshots: IRaidScreenshot[];
  prefetchLoot: RaidLootDto[];
  prefetchLootItems: LootItemDto[];
  prefetchParticipants: RaidParticipantDto[];
  prefetchUsers: IUser[];
};

const BodyBlock = ({
  raid,
  prefetchScreenshots,
  prefetchLoot,
  prefetchLootItems,
  prefetchParticipants,
  prefetchUsers,
}: Props) => {
  const [tab, setTab] = useState<number>(0);

  const { screenshots, isLoading, errorState } = useGetRaidScreenshots(
    prefetchScreenshots,
    raid.id
  );

  const tabContent: Record<number, JSX.Element> = useMemo(
    () => ({
      0: (
        <Overview
          raid={raid}
          prefetchScreenshots={prefetchScreenshots}
          prefetchParticipants={prefetchParticipants}
          prefetchLoot={prefetchLoot}
        />
      ),
      1: (
        <Participants
          raid={raid}
          prefetchParticipants={prefetchParticipants}
          prefetchUsers={prefetchUsers}
        />
      ),
      2: (
        <Screenshots
          raid={raid}
          screenshotData={{ screenshots, isLoading, errorState }}
        />
      ),
      3: (
        <Loot
          raid={raid}
          prefetchLoot={prefetchLoot}
          prefetchLootItems={prefetchLootItems}
        />
      ),
    }),
    [
      raid,
      prefetchScreenshots,
      prefetchParticipants,
      prefetchLoot,
      prefetchUsers,
      screenshots,
      isLoading,
      errorState,
      prefetchLootItems,
    ]
  );

  const content = tabContent[tab];

  return (
    <section className={styles.raidBodySection}>
      <div className={styles.wrapper}>
        <RaidTabs value={tab} setValue={setTab} />
        {content}
      </div>
    </section>
  );
};

export default BodyBlock;
