import { z } from "zod";
import { CreateSalaryRequest, SalaryPaymentType } from "@/domains/reports/types";
import { BossType } from "@/domains/raids/types";
import { formatDate, DateFormat } from "@/utils/formateDate";

export const step1Schema = z
  .object({
    startDt: z.string().min(1, "Выберите дату начала"),
    endDt: z.string().min(1, "Выберите дату окончания"),
    name: z.string().min(1, "Название периода обязательно"),
    allowedPaymentTypes: z
      .array(z.number())
      .min(1, "Выберите хотя бы одну валюту"),
    useCoefficients: z.boolean(),
    bossTypes: z.array(z.number()).min(1, "Выберите хотя бы одного босса"),
  })
  .refine((data) => new Date(data.endDt) > new Date(data.startDt), {
    message: "Дата окончания должна быть позже даты начала",
    path: ["endDt"],
  });

export type Step1FormValues = z.input<typeof step1Schema>;

export const step1Defaults: Step1FormValues = {
  startDt: "",
  endDt: "",
  name: "",
  allowedPaymentTypes: [],
  useCoefficients: false,
  bossTypes: [],
};

export function mapFormToRequest(form: Step1FormValues): CreateSalaryRequest {
  return {
    ...form,
    startDt: formatDate(form.startDt, DateFormat.ISO_DATE),
    endDt: formatDate(form.endDt, DateFormat.ISO_DATE),
    allowedPaymentTypes: form.allowedPaymentTypes as SalaryPaymentType[],
    bossTypes: form.bossTypes as BossType[],
  };
}
