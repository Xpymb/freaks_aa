import type { LootType, LootGradeType } from "@/domains/raids/types";

export interface LootItemDto {
  id: number;
  type: LootType;
  gradeType: LootGradeType;
  name: string;
  description: string;
  synthesisExp?: number;
  iconUri: string;
}

export interface LootItem {
  id: number;
  type: LootType;
  gradeType: LootGradeType;
  name: string;
  description: string;
  synthesisExp?: number;
  iconUri: string;
}
