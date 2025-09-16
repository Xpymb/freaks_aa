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
import { useGetRaidScreenshots } from "@/domains/raids/hooks/useGetScreenshot";
import { IUser } from "@/domains/user/types";
import { motion, AnimatePresence } from "framer-motion";

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
