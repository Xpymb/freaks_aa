export const ReportStatus = {
  Completed: 1,
  AwaitingParameters: 2,
} as const;
export type ReportStatus = (typeof ReportStatus)[keyof typeof ReportStatus];

export interface ReportParameters {
  periodParameters: boolean; // Параметры периода
  analyzedIncomes: boolean; // Проанализированные доходы
  ppDebts: boolean; // Долги ПП
  expensesAndDeductions: boolean; // Расходы и отчисления
  salaryDistribution: boolean; // Распределение ЗП
}

export interface ReportItem {
  id: number;
  period: string; // "20 ноя - 20 дек", "Декабрь"
  parameters: ReportParameters;
  status: ReportStatus;
}
