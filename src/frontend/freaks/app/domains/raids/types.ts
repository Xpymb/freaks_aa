import { IUser } from "@/types/user.types";

export const RaidStatus = {
  Planned: 1,
  WaitingScreenshot: 2,
  WaitingSubmit: 3,
  Ended: 4,
} as const;
export type RaidStatus = (typeof RaidStatus)[keyof typeof RaidStatus];

export const BossType = {
  Jmg: 1,
  AbyssalJmg: 2,
  Rangora: 3,
  Morpheus: 4,
  Kraken: 5,
  BlackDragon: 6,
  Charybdis: 7,
  Leviathan: 8,
  Anthalon: 9,
  AbyssalSehekmet: 10,
  Kaliel: 11,
} as const;
export type BossType = (typeof BossType)[keyof typeof BossType];

export const RaidFormatType = {
  Pve: 1,
  Pvp: 2,
} as const;
export type RaidFormatType =
  (typeof RaidFormatType)[keyof typeof RaidFormatType];

export interface RaidListItem {
  id: number;
  bossType: BossType;
  creator: IUser;
  startDt: string;
  status: RaidStatus;
  format?: RaidFormatType;
}

export interface RaidItem {
  id: number;
  bossType: BossType;
  formatType: RaidFormatType;
  creator: IUser;
  startDt: string;
  createdDt: string;
  updatedDt: string | null;
  description: string;
  status: RaidStatus;
}
