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

export interface IRaidScreenshot {
  raidId: number;
  screenshotUri: string;
}

// Enums based on API documentation
export const LootType = {
  WorldBossInfusion: 1,
  ErenorInfusion: 2,
  ArcheumCrystal: 3,
  WorldBossEquipment: 4,
  BossHearth: 5,
  Lunagem: 6,
  AwakeningScroll: 7,
  DragonHeartshard: 8,
  Glider: 9,
  Gold: 10,
  Other: 30,
} as const;
export type LootType = (typeof LootType)[keyof typeof LootType];

export const LootGradeType = {
  Crude: 1,
  Basic: 2,
  Grand: 3,
  Rare: 4,
  Arcane: 5,
  Heroic: 6,
  Unique: 7,
  Celestial: 8,
  Divine: 9,
  Epic: 10,
  Legendary: 11,
  Mythic: 12,
  Eternal: 13,
} as const;
export type LootGradeType = (typeof LootGradeType)[keyof typeof LootGradeType];

// API DTOs
export interface LootItemDto {
  id: number;
  type: LootType;
  gradeType: LootGradeType;
  name: string;
  description: string;
  synthesisExp?: number;
  iconUri: string;
}

export interface RaidLootDto {
  raidId: number;
  loot: LootItemDto;
  quantity: number;
}

// Request types
export interface CreateRaidLootRequest {
  lootId: number;
  quantity: number;
}

export interface UpdateRaidLootRequest {
  quantity: number;
}

// Raid Participants types
export interface RaidParticipantDto {
  raidId: number;
  raidNumber: number;
  raidPartyNumber: number;
  raidPartyPositionNumber: number;
  participant: IUser;
}

export interface CreateRaidParticipantRequest {
  participantId: string;
  raidNumber: number;
  raidPartyNumber: number;
  raidPartyPositionNumber: number;
}
