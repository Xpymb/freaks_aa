import type { HTMLAttributes } from "react";
import CustomImage from "@/components/ui/CustomImage";
import type { LootItemDto } from "@/domains/loot/types";
import type { Option } from "@/components/ui/formInputs/SingleAutocomplete";
import { getGradeColor } from "@/shared/utils/lootGrade";
import styles from "./_styles.module.scss";

type Props = {
  props: HTMLAttributes<HTMLLIElement>;
  option: Option<number>;
};

const LootItemOption = ({ props, option }: Props) => {
  const { key, ...liProps } = props as HTMLAttributes<HTMLLIElement> & { key?: string };
  const item = option.data as LootItemDto | undefined;

  return (
    <li {...liProps}>
      <div className={styles.option}>
        <div className={styles.iconWrapper}>
          {item?.iconUri ? (
            <div className={styles.iconContainer}>
              <CustomImage
                src={`${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${item.iconUri}`}
                alt={item.name}
                fill
                className={styles.image}
              />
              <CustomImage
                src={`/images/masks/icon_grade${item.gradeType - 1}.png`}
                alt=""
                fill
                className={styles.gradeMask}
              />
            </div>
          ) : (
            <div
              className={styles.colorFallback}
              style={{
                backgroundColor: item ? getGradeColor(item.gradeType) : "#9e9e9e",
              }}
            />
          )}
        </div>
        <div className={styles.text}>
          <div className={styles.name}>{option.label}</div>
          {item?.synthesisExp && (
            <div className={styles.synthesis}>Опыт: {item.synthesisExp}</div>
          )}
        </div>
      </div>
    </li>
  );
};

export default LootItemOption;
