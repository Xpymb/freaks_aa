import Chip from "@mui/material/Chip";
import clsx from "clsx";
import styles from "./_styles.module.scss";
import { RAID_STATUS_LABEL, RaidStatus } from "@/domains/raids";

type Props = {
  status: RaidStatus;
};

const StatusChip = ({ status }: Props) => {
  return (
    <Chip
      className={clsx(styles.statusChip, {
        [styles.planned]: status === RaidStatus.Planned,
        [styles.waitingScreenshot]: status === RaidStatus.WaitingScreenshot,
        [styles.waitingSubmit]: status === RaidStatus.WaitingSubmit,
        [styles.ended]: status === RaidStatus.Ended,
      })}
      variant="outlined"
      label={RAID_STATUS_LABEL[status]}
    />
  );
};

export default StatusChip;
