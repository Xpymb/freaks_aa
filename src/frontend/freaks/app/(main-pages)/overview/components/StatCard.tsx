"use client";

import React, { useEffect, useState } from "react";
import {
  TrendingUp as TrendingUpIcon,
  TrendingDown as TrendingDownIcon,
} from "@mui/icons-material";
import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";

type StatCardProps = {
  title: string;
  value: string;
  subtitle: string;
  icon: React.ReactNode;
  color: "primary" | "success" | "warning" | "info";
  trend: "up" | "down";
  delay?: number;
};

const StatCard = ({
  title,
  value,
  subtitle,
  icon,
  color,
  trend,
  delay = 0,
}: StatCardProps) => {
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    const timer = setTimeout(() => {
      setIsVisible(true);
    }, delay);

    return () => clearTimeout(timer);
  }, [delay]);

  return (
    <div
      className={`${styles.statCard} ${styles[color]} ${
        isVisible ? styles.visible : ""
      }`}
    >
      <div className={styles.cardBackground} />

      <div className={styles.cardHeader}>
        <div className={styles.iconWrapper}>{icon}</div>
        <div className={styles.trendIndicator}>
          {trend === "up" ? (
            <TrendingUpIcon className={styles.trendUp} />
          ) : (
            <TrendingDownIcon className={styles.trendDown} />
          )}
        </div>
      </div>

      <div className={styles.cardContent}>
        <CustomTypography variant="h3" className={styles.statValue}>
          {value}
        </CustomTypography>

        <CustomTypography variant="body1" className={styles.statTitle}>
          {title}
        </CustomTypography>

        <CustomTypography variant="body2" className={styles.statSubtitle}>
          {subtitle}
        </CustomTypography>
      </div>

      <div className={styles.cardGlow} />
    </div>
  );
};

export default StatCard;
