"use client";

import { useMemo } from "react";
import { useFormContext } from "react-hook-form";
import { CustomTypography } from "@/components";
import { CheckboxGroupField } from "@/components/ui/formInputs/CheckboxGroupField";
import type { CheckboxOption } from "@/components/ui/formInputs/CheckboxGroupField";
import { SalaryPaymentType } from "@/domains/reports/types";
import { PAYMENT_TYPE_LABEL } from "@/domains/reports/constants";
import type { Step1FormValues } from "../step1Schema";
import styles from "../_styles.module.scss";

const CurrencySection = () => {
  const { control } = useFormContext<Step1FormValues>();

  const options: CheckboxOption<number>[] = useMemo(
    () =>
      Object.entries(SalaryPaymentType).map(([, value]) => ({
        value: value as number,
        label: PAYMENT_TYPE_LABEL[value as SalaryPaymentType],
      })),
    [],
  );

  return (
    <section className={styles.section}>
      <CustomTypography variant="h6" className={styles.sectionTitle}>
        Валюта
      </CustomTypography>

      <CheckboxGroupField
        control={control}
        name="allowedPaymentTypes"
        options={options}
        columns={3}
      />
    </section>
  );
};

export default CurrencySection;
