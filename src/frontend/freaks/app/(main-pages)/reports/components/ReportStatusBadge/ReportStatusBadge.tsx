import { Chip } from "@mui/material";
import clsx from "clsx";
import {
  SALARY_REGISTRATION_LABELS,
  SalaryRegistrationStatus,
} from "@/domains/reports";
import { CustomTypography } from "@/components";
import styles from "./_styles.module.scss";

type Props = {
  status: SalaryRegistrationStatus;
  className?: string;
};

const ReportStatusBadge = ({ status, className }: Props) => {
  return (
    <Chip
      className={clsx(styles.statusBadge, className, {
        [styles.completed]: status === SalaryRegistrationStatus.Ended,
        [styles.awaitingParameters]: status === SalaryRegistrationStatus.Opened,
      })}
      variant="outlined"
      label={
        <CustomTypography variant="caption">
          {SALARY_REGISTRATION_LABELS[status]}
        </CustomTypography>
      }
    />
  );
};

export default ReportStatusBadge;
