"use client";

import { CustomTypography } from "@/components/ui/CustomTypography";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import { RaidConditionalRender } from "@/components/ui";
import { RaidItem, RaidLootDto } from "@/domains/raids";
import { LootItemDto } from "@/domains/loot";
import { useRaidPermissions } from "@/domains/raids";
import styles from "./_styles.module.scss";
import LootCard from "./LootCard";
import AddLootForm from "./AddLootForm";
import InventoryIcon from "@mui/icons-material/Inventory";
import { useAppError } from "@/shared/errors";

type Props = {
  raid: RaidItem;
  loot: RaidLootDto[];
  lootLoading: boolean;
  lootError: ReturnType<typeof useAppError>;
  prefetchLootItems: LootItemDto[];
  onLootChange?: () => void;
};

const Loot = ({
  raid,
  loot,
  lootLoading,
  lootError,
  prefetchLootItems,
  onLootChange,
}: Props) => {
  // Получаем права доступа к рейду
  const { canEdit } = useRaidPermissions(raid);

  if (lootLoading) {
    return (
      <div className={styles.lootContainer}>
        <div className={styles.lootHeader}>
          <CustomTypography variant="h4">Лут</CustomTypography>
        </div>
        <div className={styles.loadingState}>
          {[...Array(6)].map((_, i) => (
            <div key={i} className={styles.loadingCard} />
          ))}
        </div>
      </div>
    );
  }

  if (lootError?.isError) {
    return (
      <div className={styles.lootContainer}>
        <div className={styles.lootHeader}>
          <CustomTypography variant="h4">Лут</CustomTypography>
        </div>
        <ErrorLoadData message={lootError?.message} />
      </div>
    );
  }

  // Фильтруем поврежденные данные
  const validLootItems = loot.filter((item) => item && item.loot);

  return (
    <div className={styles.lootContainer}>
      {/* Форма для добавления лута - только для пользователей с правами на редактирование */}
      <RaidConditionalRender raid={raid} permission="canEdit">
        <AddLootForm
          raidId={raid.id}
          onLootAdded={onLootChange || (() => {})}
          prefetchLootItems={prefetchLootItems}
        />
      </RaidConditionalRender>

      {/* Отображение лута */}
      {validLootItems.length === 0 ? (
        <div className={styles.emptyState}>
          <InventoryIcon className={styles.emptyIcon} />
          <CustomTypography variant="h6" gutterBottom>
            Пока нет добычи
          </CustomTypography>
          <CustomTypography variant="body2">
            Добавьте первый предмет лута используя форму выше
          </CustomTypography>
        </div>
      ) : (
        <div className={styles.lootGrid}>
          {validLootItems.map((lootItem) => (
            <LootCard
              key={`${lootItem.raidId}-${lootItem.loot?.id || "unknown"}`}
              lootItem={lootItem}
              onDelete={canEdit ? onLootChange || (() => {}) : undefined}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default Loot;
