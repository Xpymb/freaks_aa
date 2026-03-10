"use client";

import { useFormContext } from "react-hook-form";
import { SwitchField } from "@/components/ui/formInputs/SwitchField";
import type { Step1FormValues } from "../step1Schema";
import styles from "../_styles.module.scss";

const CoefficientsToggle = () => {
  const { control } = useFormContext<Step1FormValues>();

  return (
    <section className={styles.section}>
      <SwitchField
        control={control}
        name="useCoefficients"
        label="Система коэффициентов"
      />
    </section>
  );
};

export default CoefficientsToggle;
