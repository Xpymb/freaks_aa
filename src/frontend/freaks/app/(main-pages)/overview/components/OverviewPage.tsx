"use client";

import React from "react";
import HeroSection from "./HeroSection";
import StatsSection from "./StatsSection";
import styles from "./_styles.module.scss";

const OverviewPage = () => {
  return (
    <div className={styles.overviewPage}>
      <HeroSection />
      <StatsSection />
    </div>
  );
};

export default OverviewPage;
