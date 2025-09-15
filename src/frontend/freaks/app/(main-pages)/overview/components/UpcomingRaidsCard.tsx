"use client";

import React from "react";
import { motion } from "framer-motion";
import { CustomTypography } from "@/components/ui/CustomTypography";
import { RaidListItem, BOSS_LABEL } from "@/domains/raids";
import { formatDate, DateFormat } from "@/utils/formateDate";
import {
  Schedule as ScheduleIcon,
  Person as PersonIcon,
  ArrowForward as ArrowIcon,
} from "@mui/icons-material";
import { useRouter } from "next/navigation";
import styles from "./_styles.module.scss";

interface UpcomingRaidsCardProps {
  raids: RaidListItem[];
  isLoading?: boolean;
}

const UpcomingRaidsCard = ({ raids, isLoading }: UpcomingRaidsCardProps) => {
  const router = useRouter();

  const containerVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: {
      opacity: 1,
      y: 0,
      transition: {
        duration: 0.6,
        staggerChildren: 0.1,
      },
    },
  };

  const itemVariants = {
    hidden: { opacity: 0, x: -20 },
    visible: {
      opacity: 1,
      x: 0,
      transition: { duration: 0.4 },
    },
  };

  if (isLoading) {
    return (
      <motion.div
        className={`${styles.bentoCard} ${styles.upcomingRaids}`}
        variants={containerVariants}
        initial="hidden"
        animate="visible"
      >
        <div className={styles.cardHeader}>
          <div className={styles.headerIcon}>
            <ScheduleIcon />
          </div>
          <div>
            <CustomTypography variant="h6" className={styles.cardTitle}>
              Ближайшие рейды
            </CustomTypography>
            <CustomTypography variant="body2" className={styles.cardSubtitle}>
              Загрузка...
            </CustomTypography>
          </div>
        </div>

        <div className={styles.raidsList}>
          {[...Array(5)].map((_, i) => (
            <div key={i} className={styles.raidSkeleton} />
          ))}
        </div>
      </motion.div>
    );
  }

  return (
    <motion.div
      className={`${styles.bentoCard} ${styles.upcomingRaids}`}
      variants={containerVariants}
      initial="hidden"
      animate="visible"
      whileHover={{ scale: 1.02 }}
    >
      <div className={styles.cardHeader}>
        <div className={styles.headerIcon}>
          <ScheduleIcon />
        </div>
        <div>
          <CustomTypography variant="h6" className={styles.cardTitle}>
            Ближайшие рейды
          </CustomTypography>
          <CustomTypography variant="body2" className={styles.cardSubtitle}>
            Следующие 30 дней
          </CustomTypography>
        </div>
      </div>

      <div className={styles.raidsList}>
        {raids.length === 0 ? (
          <div className={styles.emptyState}>
            <CustomTypography variant="body2" className={styles.emptyText}>
              Нет запланированных рейдов
            </CustomTypography>
          </div>
        ) : (
          raids.slice(0, 5).map((raid) => (
            <motion.div
              key={raid.id}
              className={styles.raidItem}
              variants={itemVariants}
              whileHover={{
                x: 5,
                backgroundColor: "rgba(255, 255, 255, 0.05)",
              }}
              onClick={() => router.push(`/raids/${raid.id}`)}
            >
              <div className={styles.raidInfo}>
                <CustomTypography
                  variant="subtitle2"
                  className={styles.raidTitle}
                >
                  {BOSS_LABEL[raid.bossType]}
                </CustomTypography>
                <div className={styles.raidMeta}>
                  <div className={styles.metaItem}>
                    <ScheduleIcon className={styles.metaIcon} />
                    <span>
                      {formatDate(
                        raid.startDt,
                        DateFormat.SHORT_DATE_SHORT_YEAR_TIME
                      )}
                    </span>
                  </div>
                  <div className={styles.metaItem}>
                    <PersonIcon className={styles.metaIcon} />
                    <span>{raid.creator.gameNickname}</span>
                  </div>
                </div>
              </div>

              <motion.div className={styles.raidAction} whileHover={{ x: 5 }}>
                <ArrowIcon className={styles.actionIcon} />
              </motion.div>
            </motion.div>
          ))
        )}
      </div>

      {raids.length > 5 && (
        <motion.div
          className={styles.viewMore}
          whileHover={{ scale: 1.05 }}
          onClick={() => router.push("/raids")}
        >
          <CustomTypography variant="body2" className={styles.viewMoreText}>
            Посмотреть все рейды
          </CustomTypography>
          <ArrowIcon className={styles.viewMoreIcon} />
        </motion.div>
      )}
    </motion.div>
  );
};

export default UpcomingRaidsCard;
