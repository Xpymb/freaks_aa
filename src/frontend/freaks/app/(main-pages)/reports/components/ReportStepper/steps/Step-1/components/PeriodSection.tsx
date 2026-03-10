"use client";

import { useFormContext } from "react-hook-form";
import { CustomTypography } from "@/components";
import { InputField } from "@/components/ui/formInputs/CustomInput";
import DateOrRangeField from "@/components/ui/formInputs/DateField/DateField";
import type { Step1FormValues } from "../step1Schema";
import styles from "../_styles.module.scss";

const PeriodSection = () => {
  const { control } = useFormContext<Step1FormValues>();

  return (
    <section className={styles.section}>
      <CustomTypography variant="h6" className={styles.sectionTitle}>
        Период
      </CustomTypography>

      <div className={styles.row}>
        <DateOrRangeField
          control={control}
          nameFrom="startDt"
          nameTo="endDt"
          label="Выберите период"
        />
      </div>

      <div className={styles.row}>
        <InputField
          control={control}
          name="name"
          label="Название периода"
          placeholder="Новый отчетный период"
        />
      </div>
    </section>
  );
};

export default PeriodSection;
