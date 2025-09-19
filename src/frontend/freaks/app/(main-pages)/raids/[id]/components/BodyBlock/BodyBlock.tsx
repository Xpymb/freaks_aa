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
import Screenshots from "../(TabSection)/Screenshots/Screenshots";
import Loot from "../(TabSection)/Loot/Loot";
import Participants from "../(TabSection)/Participants/Participants";
import { IUser } from "@/domains/user/types";
import { motion, AnimatePresence } from "framer-motion";
import { useGetRaidByID } from "@/domains/raids/hooks/useGetRaidByID";
import { useGetRaidScreenshots } from "@/domains/raids/hooks/useGetScreenshot";
import { useGetRaidParticipants } from "@/domains/raids/hooks/useGetRaidParticipants";
import { useGetRaidLoot } from "@/domains/raids/hooks/useGetRaidLoot";

type Props = {
  initialRaid: RaidItem;
  prefetchScreenshots: IRaidScreenshot[];
  prefetchLoot: RaidLootDto[];
  prefetchLootItems: LootItemDto[];
  prefetchParticipants: RaidParticipantDto[];
  prefetchUsers: IUser[];
};

const BodyBlock = ({
  initialRaid,
  prefetchScreenshots,
  prefetchLoot,
  prefetchLootItems,
  prefetchParticipants,
  prefetchUsers,
}: Props) => {
  const [tab, setTab] = useState<number>(0);

  // Хуки для получения данных
  const { raid } = useGetRaidByID(initialRaid, initialRaid.id);
  const {
    screenshots,
    isLoading: screenshotsLoading,
    errorState: screenshotsError,
  } = useGetRaidScreenshots(prefetchScreenshots, initialRaid.id);
  const {
    data: participants,
    isLoading: participantsLoading,
    errorState: participantsError,
  } = useGetRaidParticipants(prefetchParticipants, initialRaid.id);
  const {
    lootItems,
    isLoading: lootLoading,
    errorState: lootError,
  } = useGetRaidLoot(prefetchLoot, initialRaid.id);

  const tabContent: Record<number, JSX.Element> = useMemo(
    () => ({
      0: (
        <Overview
          raid={raid || initialRaid}
          participants={participants || []}
          participantsLoading={participantsLoading}
          participantsError={participantsError}
          loot={lootItems || []}
          lootLoading={lootLoading}
          lootError={lootError}
          screenshots={screenshots || []}
          screenshotsLoading={screenshotsLoading}
          screenshotsError={screenshotsError}
        />
      ),
      1: (
        <Participants
          raid={raid || initialRaid}
          participants={participants || []}
          participantsLoading={participantsLoading}
          participantsError={participantsError}
          prefetchUsers={prefetchUsers}
          screenshots={screenshots || []}
          onParticipantsChange={() => {
            // Можно добавить логику обновления если нужно
          }}
        />
      ),
      2: (
        <Screenshots
          raid={raid || initialRaid}
          screenshotData={{
            screenshots,
            isLoading: screenshotsLoading,
            errorState: screenshotsError,
          }}
        />
      ),
      3: (
        <Loot
          raid={raid || initialRaid}
          loot={lootItems || []}
          lootLoading={lootLoading}
          lootError={lootError}
          prefetchLootItems={prefetchLootItems}
          onLootChange={() => {
            // Можно добавить логику обновления если нужно
          }}
        />
      ),
    }),
    [
      raid,
      initialRaid,
      prefetchUsers,
      screenshots,
      screenshotsLoading,
      screenshotsError,
      participants,
      participantsLoading,
      participantsError,
      lootItems,
      lootLoading,
      lootError,
      prefetchLootItems,
    ]
  );

  const content = tabContent[tab];

  if (!raid) return null;

  const containerVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: {
      opacity: 1,
      y: 0,
      transition: {
        duration: 0.6,
        ease: "easeOut" as const,
        staggerChildren: 0.1,
      },
    },
  };

  const itemVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: {
      opacity: 1,
      y: 0,
      transition: {
        duration: 0.5,
        ease: "easeOut" as const,
      },
    },
  };

  return (
    <motion.section
      className={styles.raidBodySection}
      variants={containerVariants}
      initial="hidden"
      animate="visible"
    >
      <motion.div className={styles.wrapper} variants={itemVariants}>
        <RaidTabs value={tab} setValue={setTab} />
        <AnimatePresence mode="wait">
          <motion.div
            key={tab}
            initial={{ opacity: 0, x: 20 }}
            animate={{ opacity: 1, x: 0 }}
            exit={{ opacity: 0, x: -20 }}
            transition={{ duration: 0.3, ease: "easeInOut" as const }}
          >
            {content}
          </motion.div>
        </AnimatePresence>
      </motion.div>
    </motion.section>
  );
};

export default BodyBlock;
