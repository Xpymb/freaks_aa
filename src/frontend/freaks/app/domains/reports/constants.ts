import { ReportStatus } from "./types";

export const REPORT_STATUS_LABEL: Record<ReportStatus, string> = {
  [ReportStatus.Completed]: "Период завершен",
  [ReportStatus.AwaitingParameters]: "Ожидает заполнения всех параметров",
};

export const REPORT_PARAMETER_LABELS = {
  periodParameters: "Параметры периода",
  analyzedIncomes: "Проанализированные доходы",
  ppDebts: "Долги ПП",
  expensesAndDeductions: "Расходы и отчисления",
  salaryDistribution: "Распределение ЗП",
};
