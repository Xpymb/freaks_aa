"use client";

import React from "react";
import { motion } from "framer-motion";
import { CustomTypography } from "@/components/ui/CustomTypography";
import {
  ShoppingCart as ShopIcon,
  Gavel as AuctionIcon,
  Construction as ConstructionIcon,
} from "@mui/icons-material";
import styles from "./_styles.module.scss";

interface DisabledFeatureCardProps {
  type: "shop" | "auction";
  className?: string;
}

const DisabledFeatureCard = ({
  type,
  className = "",
}: DisabledFeatureCardProps) => {
  const config = {
    shop: {
      title: "Магазин",
      subtitle: "Покупка предметов",
      description:
        "Скоро здесь можно будет покупать редкие предметы и экипировку",
      icon: <ShopIcon />,
      color: "shopCard",
    },
    auction: {
      title: "Аукцион",
      subtitle: "Торговля с игроками",
      description: "Система аукциона для торговли предметами между игроками",
      icon: <AuctionIcon />,
      color: "auctionCard",
    },
  };

  const { title, subtitle, description, icon, color } = config[type];

  const cardVariants = {
    hidden: { opacity: 0, scale: 0.9 },
    visible: {
      opacity: 1,
      scale: 1,
      transition: {
        duration: 0.6,
        ease: "easeOut" as const,
      },
    },
  };

  return (
    <motion.div
      className={`${styles.bentoCard} ${styles.disabledFeature} ${styles[color]} ${className}`}
      variants={cardVariants}
      initial="hidden"
      animate="visible"
      whileHover={{
        scale: 1.02,
        transition: { duration: 0.2 },
      }}
    >
      <div className={styles.disabledOverlay}>
        <ConstructionIcon className={styles.constructionIcon} />
        <CustomTypography variant="caption" className={styles.disabledLabel}>
          В разработке
        </CustomTypography>
      </div>

      <div className={styles.cardHeader}>
        <div className={styles.headerIcon}>{icon}</div>
        <div>
          <CustomTypography variant="h6" className={styles.cardTitle}>
            {title}
          </CustomTypography>
          <CustomTypography variant="body2" className={styles.cardSubtitle}>
            {subtitle}
          </CustomTypography>
        </div>
      </div>

      <div className={styles.featureContent}>
        <CustomTypography variant="body2" className={styles.featureDescription}>
          {description}
        </CustomTypography>

        <div className={styles.comingSoon}>
          <motion.div
            className={styles.pulseRing}
            animate={{
              scale: [1, 1.2, 1],
              opacity: [0.7, 0.3, 0.7],
            }}
            transition={{
              duration: 2,
              repeat: Infinity,
              ease: "easeInOut",
            }}
          />
          <CustomTypography
            variant="subtitle2"
            className={styles.comingSoonText}
          >
            Скоро...
          </CustomTypography>
        </div>
      </div>
    </motion.div>
  );
};

export default DisabledFeatureCard;
