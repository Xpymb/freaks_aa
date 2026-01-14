import { ReportItem, ReportStatus } from "./types";

export const MOCK_REPORTS: ReportItem[] = [
  {
    id: 1,
    period: "20 ноя - 20 дек",
    parameters: {
      periodParameters: true,
      analyzedIncomes: false,
      ppDebts: true,
      expensesAndDeductions: true,
      salaryDistribution: false,
    },
    status: ReportStatus.AwaitingParameters,
  },
  {
    id: 2,
    period: "20 ноя - 20 дек",
    parameters: {
      periodParameters: true,
      analyzedIncomes: true,
      ppDebts: true,
      expensesAndDeductions: true,
      salaryDistribution: true,
    },
    status: ReportStatus.Completed,
  },
  {
    id: 3,
    period: "Декабрь",
    parameters: {
      periodParameters: true,
      analyzedIncomes: true,
      ppDebts: true,
      expensesAndDeductions: true,
      salaryDistribution: true,
    },
    status: ReportStatus.Completed,
  },
  {
    id: 4,
    period: "Ноябрь+окт",
    parameters: {
      periodParameters: true,
      analyzedIncomes: true,
      ppDebts: true,
      expensesAndDeductions: true,
      salaryDistribution: true,
    },
    status: ReportStatus.Completed,
  },
  {
    id: 5,
    period: "20 ноя - 20 дек",
    parameters: {
      periodParameters: true,
      analyzedIncomes: true,
      ppDebts: true,
      expensesAndDeductions: true,
      salaryDistribution: true,
    },
    status: ReportStatus.Completed,
  },
  {
    id: 6,
    period: "20 ноя - 20 дек",
    parameters: {
      periodParameters: true,
      analyzedIncomes: true,
      ppDebts: true,
      expensesAndDeductions: true,
      salaryDistribution: true,
    },
    status: ReportStatus.Completed,
  },
];
