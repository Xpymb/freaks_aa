import { BossType, RaidFormatType, RaidStatus } from "./types";

export const BOSS_LABEL: Record<BossType, string> = {
  1: "АГЛ",
  2: "Т2 АГЛ",
  3: "Марли",
  4: "Морф",
  5: "Кракен",
  6: "Ксанатос",
  7: "Калидис",
  8: "Левиафан",
  9: "Анталлон",
  10: "Кошка",
  11: "Калиель",
};

export const RAID_STATUS_LABEL: Record<RaidStatus, string> = {
  1: "Запланирован",
  2: "В ожидании скриншота",
  3: "В ожидании подтверждения",
  4: "Завершён",
};

export const RAID_FORMAT_LABEL: Record<RaidFormatType, string> = {
  1: "PvE",
  2: "PvP",
};
