import { LootGradeType } from "@/domains/raids/types";

export const getGradeColor = (gradeType: LootGradeType): string => {
  switch (gradeType) {
    case LootGradeType.Crude:
    case LootGradeType.Basic:
      return "#9e9e9e";
    case LootGradeType.Grand:
    case LootGradeType.Rare:
      return "#2196f3";
    case LootGradeType.Arcane:
    case LootGradeType.Heroic:
    case LootGradeType.Unique:
      return "#4caf50";
    case LootGradeType.Celestial:
    case LootGradeType.Divine:
    case LootGradeType.Epic:
      return "#9c27b0";
    case LootGradeType.Legendary:
    case LootGradeType.Mythic:
    case LootGradeType.Eternal:
      return "#ff9800";
    default:
      return "#9e9e9e";
  }
};
