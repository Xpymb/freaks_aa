"use client";

import React from "react";
import { Button } from "@mui/material";
import {
  Rocket as RocketIcon,
  TrendingUp as TrendingIcon,
  Shield as ShieldIcon,
  AutoAwesome as SparkleIcon,
} from "@mui/icons-material";
import { useRouter } from "next/navigation";
import { motion } from "framer-motion";
import { CustomTypography } from "@/components/ui/CustomTypography";
import { CustomContainer } from "@/components/ui/CustomContainer";
import styles from "./_styles.module.scss";

const HeroSection = () => {
  const router = useRouter();

  const containerVariants = {
    hidden: { opacity: 0 },
    visible: {
      opacity: 1,
      transition: {
        delayChildren: 0.2,
        staggerChildren: 0.1,
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

  const iconVariants = {
    hidden: { scale: 0, rotate: -180 },
    visible: {
      scale: 1,
      rotate: 0,
      transition: {
        duration: 0.8,
        ease: "easeOut" as const,
      },
    },
    hover: {
      scale: 1.1,
      rotate: 15,
      transition: {
        duration: 0.3,
      },
    },
  };

  const floatingVariants = {
    float: {
      y: [-20, 20, -20],
      rotate: [0, 180, 360],
      transition: {
        duration: 6,
        repeat: Infinity,
        ease: "easeInOut" as const,
      },
    },
  };

  return (
    <section className={styles.heroSection}>
      <div className={styles.heroBackground}>
        <div className={styles.heroOverlay} />
        <div className={styles.animatedElements}>
          <motion.div
            className={styles.floatingElement}
            variants={floatingVariants}
            animate="float"
          />
          <motion.div
            className={styles.floatingElement}
            variants={floatingVariants}
            animate="float"
            transition={{ delay: 2 }}
          />
          <motion.div
            className={styles.floatingElement}
            variants={floatingVariants}
            animate="float"
            transition={{ delay: 4 }}
          />
        </div>
      </div>

      <CustomContainer maxWidth="lg">
        <motion.div
          className={styles.heroContent}
          variants={containerVariants}
          initial="hidden"
          animate="visible"
        >
          <div className={styles.heroText}>
            <motion.div className={styles.heroIcon} variants={itemVariants}>
              <motion.div
                variants={iconVariants}
                whileHover="hover"
                className={styles.iconContainer}
              >
                <ShieldIcon className={styles.guildIcon} />
                <motion.div
                  className={styles.sparkleContainer}
                  animate={{
                    rotate: 360,
                  }}
                  transition={{
                    duration: 8,
                    repeat: Infinity,
                    ease: "linear",
                  }}
                >
                  <SparkleIcon className={styles.sparkle} />
                </motion.div>
              </motion.div>
            </motion.div>

            <motion.div variants={itemVariants}>
              <CustomTypography variant="h1" className={styles.heroTitle}>
                Гэри продает
                <motion.span
                  className={styles.guildName}
                  initial={{ opacity: 0, scale: 0.8 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{
                    delay: 0.8,
                    duration: 0.8,
                    ease: "easeOut" as const,
                  }}
                >
                  {" "}
                  ТАНДЕМ (писать в лс - vellialhelheim)
                </motion.span>
              </CustomTypography>
            </motion.div>

            <motion.div variants={itemVariants}>
              <CustomTypography variant="h5" className={styles.heroSubtitle}>
                Планируй рейды, отслеживай прогресс, координируй участников.
                <br />
                Все инструменты для эффективного управления гильдией в одном
                месте.
              </CustomTypography>
            </motion.div>

            <motion.div className={styles.heroFeatures} variants={itemVariants}>
              <motion.div
                className={styles.feature}
                whileHover={{
                  scale: 1.05,
                  x: 10,
                  transition: { duration: 0.2 },
                }}
              >
                <RocketIcon className={styles.featureIcon} />
                <span>Планирование рейдов</span>
              </motion.div>
              <motion.div
                className={styles.feature}
                whileHover={{
                  scale: 1.05,
                  x: 10,
                  transition: { duration: 0.2 },
                }}
              >
                <TrendingIcon className={styles.featureIcon} />
                <span>Аналитика прогресса</span>
              </motion.div>
            </motion.div>

            <motion.div className={styles.heroActions} variants={itemVariants}>
              <motion.div
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
              >
                <Button
                  variant="contained"
                  size="large"
                  onClick={() => router.push("/raids")}
                  className={styles.heroPrimaryButton}
                >
                  Перейти к рейдам
                </Button>
              </motion.div>
            </motion.div>
          </div>

          {/* <motion.div className={styles.heroVisual} variants={itemVariants}>
            <motion.div
              className={styles.heroCard}
              whileHover={{
                scale: 1.05,
                rotateY: 5,
                transition: { duration: 0.3 },
              }}
              animate={{
                y: [-10, 10, -10],
                transition: {
                  duration: 4,
                  repeat: Infinity,
                  ease: "easeInOut",
                },
              }}
            >
              <div className={styles.cardGlow} />
              <div className={styles.cardContent}>
                <CustomTypography variant="h6" className={styles.cardTitle}>
                  Активные рейды
                </CustomTypography>
                <motion.div
                  initial={{ scale: 0 }}
                  animate={{ scale: 1 }}
                  transition={{
                    delay: 1.2,
                    duration: 0.5,
                    ease: "easeOut" as const,
                  }}
                >
                  <CustomTypography variant="h3" className={styles.cardNumber}>
                    <motion.span
                      className={styles.countUp}
                      initial={{ opacity: 0, y: 20 }}
                      animate={{ opacity: 1, y: 0 }}
                      transition={{ delay: 1.5, duration: 0.6 }}
                    >
                      24
                    </motion.span>
                  </CustomTypography>
                </motion.div>
              </div>
            </motion.div>
          </motion.div> */}
        </motion.div>
      </CustomContainer>
    </section>
  );
};

export default HeroSection;
