import {Chip} from "@mui/material";
import clsx from "clsx";
import {REPORT_STATUS_LABEL, ReportStatus} from "@/domains/reports";
import {CustomTypography} from "@/components";
import styles from "./_styles.module.scss";

type Props = {
  status: ReportStatus;
  className?: string;
};

const ReportStatusBadge = ({ status, className }: Props) => {
  return (
    <Chip
      className={clsx(styles.statusBadge, className, {
        [styles.completed]: status === ReportStatus.Completed,
        [styles.awaitingParameters]: status === ReportStatus.AwaitingParameters,
      })}
      variant="outlined"
      label={
        <CustomTypography variant="caption">
          {REPORT_STATUS_LABEL[status]}
        </CustomTypography>
      }
    />
  );
};

export default ReportStatusBadge;
