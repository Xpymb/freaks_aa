"use client";

import { useMemo } from "react";
import { useFormContext } from "react-hook-form";
import { CustomTypography } from "@/components";
import { CheckboxGroupField } from "@/components/ui/formInputs/CheckboxGroupField";
import type { CheckboxOption } from "@/components/ui/formInputs/CheckboxGroupField";
import { BossType } from "@/domains/raids/types";
import { SALARY_BOSS_LABEL } from "@/domains/reports/constants";
import type { Step1FormValues } from "../step1Schema";
import styles from "../_styles.module.scss";

const BossesSection = () => {
  const { control } = useFormContext<Step1FormValues>();

  const options: CheckboxOption<number>[] = useMemo(
    () =>
      Object.entries(BossType).map(([, value]) => ({
        value: value as number,
        label: SALARY_BOSS_LABEL[value as BossType],
      })),
    [],
  );

  return (
    <section className={styles.section}>
      <div>
        <CustomTypography variant="h6" className={styles.sectionTitle}>
          Боссы
        </CustomTypography>
        <CustomTypography variant="caption" sx={{ color: "rgba(255,255,255,0.45)" }}>
          Выбранные боссы будут учтены при формировании итогового отчета
        </CustomTypography>
      </div>

      <CheckboxGroupField
        control={control}
        name="bossTypes"
        options={options}
        columns={3}
      />
    </section>
  );
};

export default BossesSection;
