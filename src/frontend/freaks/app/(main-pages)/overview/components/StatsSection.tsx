"use client";

import React, { useEffect, useState } from "react";
import { motion } from "framer-motion";
import { CustomTypography } from "@/components/ui/CustomTypography";
import { CustomContainer } from "@/components/ui/CustomContainer";
import { RaidListItem } from "@/domains/raids";
import { RaidsService, RaidListQuery } from "@/domains/raids/raids.service";
import { buildRaidsQuery } from "@/domains/raids/hooks/useGetRaids";
import { useTokens } from "@/store/authTokenStore";
import UpcomingRaidsCard from "./UpcomingRaidsCard";
import DisabledFeatureCard from "./DisabledFeatureCard";
import styles from "./_styles.module.scss";
import { DateFormat, formatDate } from "@/utils/formateDate";
import DetailContainer from "@/components/ui/DetailContainer/DetailContainer";

const StatsSection = () => {
  const [upcomingRaids, setUpcomingRaids] = useState<RaidListItem[]>([]);
  const [isLoadingRaids, setIsLoadingRaids] = useState(true);
  const { accessToken } = useTokens();

  useEffect(() => {
    const fetchUpcomingRaids = async () => {
      if (!accessToken) return;

      try {
        const now = new Date();
        const futureDate = new Date(now.getTime() + 30 * 24 * 60 * 60 * 1000);

        const filters: Partial<RaidListQuery> = {
          From: formatDate(now, DateFormat.ISO_DATE),
          To: formatDate(futureDate, DateFormat.ISO_DATE),
          SortBy: 0, // по дате
          SortMode: 1, // ASC
          Take: 5,
          Skip: 0,
          Statuses: [1],
        };

        const query = buildRaidsQuery(filters);
        const response = await RaidsService.getRaids(accessToken, query);

        setUpcomingRaids(response.items || []);
      } catch (error) {
        console.error("Failed to fetch upcoming raids:", error);
        setUpcomingRaids([]);
      } finally {
        setIsLoadingRaids(false);
      }
    };

    fetchUpcomingRaids();
  }, [accessToken]);

  const containerVariants = {
    hidden: { opacity: 0 },
    visible: {
      opacity: 1,
      transition: {
        staggerChildren: 0.1,
        delayChildren: 0.2,
      },
    },
  };

  const itemVariants = {
    hidden: { y: 30, opacity: 0 },
    visible: {
      y: 0,
      opacity: 1,
      transition: {
        duration: 0.6,
        ease: "easeOut" as const,
      },
    },
  };

  return (
    <section className={styles.statsSection}>
      <DetailContainer>
        <motion.div
          className={styles.statsHeader}
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6 }}
        >
          <CustomTypography variant="h3" className={styles.statsTitle}>
            Активность гильдии
          </CustomTypography>
          <CustomTypography variant="h6" className={styles.statsSubtitle}>
            Ближайшие события и функции
          </CustomTypography>
        </motion.div>

        <motion.div
          className={styles.bentoGrid}
          variants={containerVariants}
          initial="hidden"
          animate="visible"
        >
          {/* Ближайшие рейды - основной блок */}
          <motion.div
            className={styles.upcomingRaidsContainer}
            variants={itemVariants}
          >
            <UpcomingRaidsCard
              raids={upcomingRaids}
              isLoading={isLoadingRaids}
            />
          </motion.div>

          {/* Боковая панель с заглушками функций */}
          <motion.div className={styles.sidePanel} variants={itemVariants}>
            <motion.div className={styles.disabledShop} variants={itemVariants}>
              <DisabledFeatureCard type="shop" />
            </motion.div>

            <motion.div
              className={styles.disabledAuction}
              variants={itemVariants}
            >
              <DisabledFeatureCard type="auction" />
            </motion.div>
          </motion.div>
        </motion.div>
      </DetailContainer>
    </section>
  );
};

export default StatsSection;
