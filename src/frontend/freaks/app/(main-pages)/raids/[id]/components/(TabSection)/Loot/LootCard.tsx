import { RaidLootDto, RaidLootService } from "@/domains/raids";
import { CustomTypography } from "@/components/ui/CustomTypography";
import CustomImage from "@/components/ui/CustomImage";
import { IconButton } from "@mui/material";
import { Delete } from "@mui/icons-material";
import { useAuth } from "@/store/authTokenStore";
import { useState } from "react";
import LootItemTooltip from "./LootItemTooltip";
import styles from "./_styles.module.scss";
import clsx from "clsx";

type Props = {
  lootItem: RaidLootDto;
  onDelete?: () => void;
};

const LootCard = ({ lootItem, onDelete }: Props) => {
  const { loot: item, quantity } = lootItem;
  const [isDeleting, setIsDeleting] = useState(false);
  const { accessToken } = useAuth();

  // Защита от null
  if (!item) {
    return null;
  }

  const handleDelete = async () => {
    if (!accessToken || !onDelete) return;

    setIsDeleting(true);
    try {
      await RaidLootService.deleteRaidLoot(
        accessToken,
        lootItem.raidId,
        item.id
      );
      onDelete();
    } catch (error) {
      console.error("Ошибка при удалении лута:", error);
    } finally {
      setIsDeleting(false);
    }
  };

  return (
    <LootItemTooltip lootItem={item}>
      <div
        className={clsx(
          styles.lootCard,
          // getGradeClass(item.gradeType),
          { [styles.manageable]: !!onDelete }
        )}
      >
        <div className={styles.iconWrapper}>
          {item.iconUri ? (
            <div className={styles.iconContainer}>
              <CustomImage
                src={`${process.env.NEXT_PUBLIC_STORAGE_MEDIA_URL}/${item.iconUri}`}
                alt={item.name}
                fill
                className={styles.icon}
              />
              <CustomImage
                src={`/images/masks/icon_grade${item.gradeType - 1}.png`}
                alt="Grade mask"
                fill
                className={styles.gradeMask}
              />
            </div>
          ) : (
            <div className={styles.colorIcon} />
          )}
        </div>

        <div className={styles.content}>
          <CustomTypography
            variant="subtitle1"
            className={styles.itemName}
            title={item.name}
          >
            {item.name}
          </CustomTypography>
          {item.description && (
            <CustomTypography
              variant="caption"
              className={styles.itemDescription}
              title={item.description}
            >
              {item.description}
            </CustomTypography>
          )}
          {item.synthesisExp && (
            <CustomTypography variant="caption" className={styles.synthesisExp}>
              Опыт синтеза: {item.synthesisExp}
            </CustomTypography>
          )}
        </div>

        <div className={styles.quantity}>
          <CustomTypography variant="h6" fontWeight={600}>
            {quantity}
          </CustomTypography>
        </div>

        {/* Actions для управления лутом */}
        {onDelete && (
          <div className={styles.manageActions}>
            <IconButton
              onClick={handleDelete}
              disabled={isDeleting}
              size="small"
              className={clsx(styles.actionButton, styles.deleteButton)}
              title="Удалить предмет"
            >
              <Delete fontSize="small" />
            </IconButton>
          </div>
        )}
      </div>
    </LootItemTooltip>
  );
};

export default LootCard;
