// export const ReportStatus = {
//   Completed: 1,
//   AwaitingParameters: 2,
// } as const;
// export type ReportStatus = (typeof ReportStatus)[keyof typeof ReportStatus];

// export interface ReportParameters {
//   periodParameters: boolean;
//   analyzedIncomes: boolean;
//   ppDebts: boolean;
//   expensesAndDeductions: boolean;
//   salaryDistribution: boolean;
// }

// export interface ReportItem {
//   id: number;
//   period: string;
//   parameters: ReportParameters;
//   status: ReportStatus;
// }

/* ─── Salary ─── */

import { BossType } from "@/domains/raids";

export const SalaryRegistrationStatus = {
  NotStarted: 1,
  Opened: 2,
  Ended: 3,
} as const;
export type SalaryRegistrationStatus =
  (typeof SalaryRegistrationStatus)[keyof typeof SalaryRegistrationStatus];

export const SalaryFillStepType = {
  Parameters: 1,
  SoldByPeriod: 10,
  GuildLeaderSalary: 20,
  Expenses: 30,
  FinalReports: 40,
} as const;
export type SalaryFillStepType =
  (typeof SalaryFillStepType)[keyof typeof SalaryFillStepType];

export const SalaryPaymentType = {
  Gold: 1,
  WorldBossInfusion: 2,
  ErenorInfusion: 3,
} as const;
export type SalaryPaymentType =
  (typeof SalaryPaymentType)[keyof typeof SalaryPaymentType];

export interface CreateSalaryRequest {
  name: string;
  startDt: string;
  endDt: string;
  allowedPaymentTypes: SalaryPaymentType[];
  useCoefficients: boolean;
  bossTypes: BossType[];
}

export interface UpdateSalaryRequest {
  name?: string;
  startDt?: string;
  endDt?: string;
  allowedPaymentTypes: SalaryPaymentType[];
  useCoefficients: boolean;
  bossTypes: BossType[];
}

export interface SalaryDto {
  id: number;
  name: string;
  startDt: string;
  endDt: string;
  fillStepType: SalaryFillStepType;
  registrationStatus: SalaryRegistrationStatus;
  allowedPaymentTypes: SalaryPaymentType[];
  useCoefficients: boolean;
  bossTypes: BossType[];
}

/* ─── SalaryLoot ─── */

import type { LootItemDto } from "@/domains/loot/types";

export interface SalaryLootDto {
  id: number;
  salaryId: number;
  lootItem: LootItemDto;
  quantity: number;
  pricePerItem: number;
  discountPercent: number;
  amount: number;
}

export interface CreateSalaryLootRequest {
  lootId: number;
  quantity: number;
  pricePerItem: number;
  discountPercent: number;
  amount: number;
}

export interface UpdateSalaryLootRequest {
  lootId: number;
  quantity: number;
  pricePerItem: number;
  discountPercent: number;
  amount: number;
}

/* ─── SalaryGuildLeader ─── */

export interface SalaryGuildLeaderDto {
  salaryLoot: SalaryLootDto;
  salaryId: number;
  quantity: number;
  amount: number;
}

export interface CreateSalaryGuildLeaderRequest {
  salaryLootId: number;
  quantity: number;
}

export interface UpdateSalaryGuildLeaderRequest {
  quantity: number;
}
