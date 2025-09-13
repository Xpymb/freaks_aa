"use client";

import { CustomTypography } from "@/components/ui/CustomTypography";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import { RaidItem, RaidLootDto, LootItemDto } from "@/domains/raids";
import { useGetRaidLoot } from "@/domains/raids/hooks/useGetRaidLoot";
import styles from "./_styles.module.scss";
import LootCard from "./LootCard";
import AddLootForm from "./AddLootForm";
import InventoryIcon from "@mui/icons-material/Inventory";

type Props = {
  raid: RaidItem;
  prefetchLoot: RaidLootDto[];
  prefetchLootItems: LootItemDto[];
};

const Loot = ({ raid, prefetchLoot, prefetchLootItems }: Props) => {
  const { lootItems, isLoading, errorState, refresh } = useGetRaidLoot(
    prefetchLoot,
    raid.id
  );


  if (isLoading) {
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

  if (errorState.isError) {
    return (
      <div className={styles.lootContainer}>
        <div className={styles.lootHeader}>
          <CustomTypography variant="h4">Лут</CustomTypography>
        </div>
        <ErrorLoadData message={errorState.message} />
      </div>
    );
  }

  // Фильтруем поврежденные данные
  const validLootItems = lootItems.filter(item => item && item.loot);
  
  const totalItems = validLootItems.reduce((sum, item) => sum + item.quantity, 0);
  const uniqueItems = validLootItems.length;

  return (
    <div className={styles.lootContainer}>
      {/* Форма для добавления лута */}
      <AddLootForm raidId={raid.id} onLootAdded={refresh} prefetchLootItems={prefetchLootItems} />

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
              key={`${lootItem.raidId}-${lootItem.loot?.id || 'unknown'}`} 
              lootItem={lootItem} 
              onDelete={refresh}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default Loot;
