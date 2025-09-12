import {
  BOSS_LABEL,
  BossType,
  RAID_STATUS_LABEL,
  RaidListItem,
  RaidStatus,
} from "@/domains/raids";
import styles from "./_styles.module.scss";
import { CustomTypography } from "@/components/ui/CustomTypography";
import { DateFormat, formatDate } from "@/utils/formateDate";
import { Chip } from "@mui/material";
import clsx from "clsx";
import StatusChip from "../StatusChip/StatusChip";

type Props = {
  raid: RaidListItem;
};

const RaidCard = ({ raid }: Props) => {
  return (
    <div className={styles.raid}>
      <div className={styles.left}>
        <Chip
          className={clsx(styles.bossChip, {
            [styles.jmg]: raid.bossType === BossType.Jmg,
            [styles.abyssalJmg]: raid.bossType === BossType.AbyssalJmg,
            [styles.rangora]: raid.bossType === BossType.Rangora,
            [styles.morpheus]: raid.bossType === BossType.Morpheus,
            [styles.kraken]: raid.bossType === BossType.Kraken,
            [styles.blackDragon]: raid.bossType === BossType.BlackDragon,
            [styles.charybdis]: raid.bossType === BossType.Charybdis,
            [styles.leviathan]: raid.bossType === BossType.Leviathan,
            [styles.anthalon]: raid.bossType === BossType.Anthalon,
            [styles.abyssalSehekmet]:
              raid.bossType === BossType.AbyssalSehekmet,
          })}
          variant="outlined"
          label={
            <CustomTypography fontWeight={600} variant="subtitle2">
              {BOSS_LABEL[raid.bossType]}
            </CustomTypography>
          }
        />
        <CustomTypography variant="subtitle1">
          {raid.creator.gameNickname}
        </CustomTypography>
      </div>
      <div className={styles.right}>
        <CustomTypography variant="subtitle1">
          {formatDate(raid.startDt, DateFormat.SHORT_DATE_SHORT_YEAR_TIME)}
        </CustomTypography>
        <StatusChip status={raid.status} />
      </div>
    </div>
  );
};

export default RaidCard;
