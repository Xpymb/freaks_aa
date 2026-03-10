import CustomImage from "@/components/ui/CustomImage";
import type { LootItemDto } from "@/domains/loot/types";
import { getGradeColor } from "@/shared/utils/lootGrade";
import styles from "./_styles.module.scss";

type Props = {
  item: LootItemDto | undefined;
};

const LootItemCell = ({ item }: Props) => {
  return (
    <div className={styles.cell}>
      <div className={styles.iconWrapper}>
        {item?.iconUri ? (
          <>
            <CustomImage
              src={`${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${item.iconUri}`}
              alt={item.name}
              fill
              className={styles.icon}
            />
            <CustomImage
              src={`/images/masks/icon_grade${item.gradeType - 1}.png`}
              alt=""
              fill
              className={styles.gradeMask}
            />
          </>
        ) : (
          <div
            className={styles.fallback}
            style={{ backgroundColor: item ? getGradeColor(item.gradeType) : "#9e9e9e" }}
          />
        )}
      </div>
      <span className={styles.name}>{item?.name}</span>
    </div>
  );
};

export default LootItemCell;
