import { SalaryPaymentType, SalaryRegistrationStatus } from "./types";
import { BossType } from "@/domains/raids/types";

// export const SALARY_STATUS_LABEL: Record<ReportStatus, string> = {
//   [ReportStatus.Completed]: "Период завершен",
//   [ReportStatus.AwaitingParameters]: "Ожидает заполнения всех параметров",
// };

export const SALARY_PARAMETER_LABELS = {
  periodParameters: "Параметры периода",
  analyzedIncomes: "Проанализированные доходы",
  ppDebts: "Долги ПП",
  expensesAndDeductions: "Расходы и отчисления",
  salaryDistribution: "Распределение ЗП",
};

export const SALARY_REGISTRATION_LABELS: Record<
  SalaryRegistrationStatus,
  string
> = {
  [SalaryRegistrationStatus.NotStarted]: "регистрация не начата",
  [SalaryRegistrationStatus.Opened]: "идет регистрация",
  [SalaryRegistrationStatus.Ended]: "регистрация завершена",
};

export const PAYMENT_TYPE_LABEL: Record<SalaryPaymentType, string> = {
  [SalaryPaymentType.Gold]: "Золото",
  [SalaryPaymentType.WorldBossInfusion]: "Эссенции ярости",
  [SalaryPaymentType.ErenorInfusion]: "Трофейные эссенции стихий",
};

export const SALARY_BOSS_LABEL: Record<BossType, string> = {
  [BossType.Jmg]: "АГЛ",
  [BossType.AbyssalJmg]: "Т2 АГЛ",
  [BossType.Rangora]: "Марли",
  [BossType.Morpheus]: "Морф",
  [BossType.Kraken]: "Кракен",
  [BossType.BlackDragon]: "Ксанатос",
  [BossType.Charybdis]: "Калидис",
  [BossType.Leviathan]: "Левиафан",
  [BossType.Anthalon]: "Анталлон",
  [BossType.AbyssalSehekmet]: "Кошка",
  [BossType.Kaliel]: "Калиель",
  [BossType.Risopoda]: "Жук",
};
