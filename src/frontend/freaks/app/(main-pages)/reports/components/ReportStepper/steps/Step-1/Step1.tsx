"use client";

import { CustomTypography } from "@/components";
import PeriodSection from "./components/PeriodSection";
import CurrencySection from "./components/CurrencySection";
import CoefficientsToggle from "./components/CoefficientsToggle";
import BossesSection from "./components/BossesSection";
import type { StepProps } from "../../ReportStepper";
import styles from "./_styles.module.scss";

const Step1 = ({ salaryId }: StepProps) => {
  return (
    <div className={styles.wrapper}>
      <CustomTypography variant="h4" className={styles.title}>
        Параметры отчетного периода
      </CustomTypography>

      <div className={styles.form}>
        <PeriodSection />
        <CurrencySection />
        <CoefficientsToggle />
        <BossesSection />
      </div>

      <CustomTypography variant="caption" className={styles.requiredHint}>
        * — поля, обязательные для заполнения
      </CustomTypography>
    </div>
  );
};

export default Step1;
