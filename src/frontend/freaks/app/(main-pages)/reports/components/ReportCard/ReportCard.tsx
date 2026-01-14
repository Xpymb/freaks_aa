import {REPORT_PARAMETER_LABELS, ReportItem} from "@/domains/reports";
import {CustomTypography} from "@/components";
import {Button, IconButton} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import RadioButtonUncheckedIcon from "@mui/icons-material/RadioButtonUnchecked";
import ReportStatusBadge from "../ReportStatusBadge/ReportStatusBadge";
import styles from "./_styles.module.scss";

type Props = {
  report: ReportItem;
  onEdit: (reportId: number) => void;
};

const ReportCard = ({ report, onEdit }: Props) => {
  return (
    <div className={styles.report}>
        <div className={styles.left}>
            <div className={styles.period}>
                <CustomTypography variant="h4" fontWeight={600}>
                    {report.period}
                </CustomTypography>
            </div>

            <div className={styles.steps}>
                {Object.entries(report.parameters).map(([key, completed]) => (
                    <div key={key} className={styles.parameter}>
                        {completed ? (
                            <CheckCircleIcon className={styles.iconCompleted} />
                        ) : (
                            <RadioButtonUncheckedIcon className={styles.iconPending} />
                        )}
                        <CustomTypography variant="caption">
                            {
                                REPORT_PARAMETER_LABELS[
                                    key as keyof typeof REPORT_PARAMETER_LABELS
                                    ]
                            }
                        </CustomTypography>
                    </div>
                ))}
            </div>
        </div>


      {/* Правая часть - Статус и кнопки */}
      <div className={styles.right}>
        <ReportStatusBadge status={report.status} />
          <div className={styles.controlBtn}>
              <Button variant="outlined" disabled className={styles.fillButton}>
                  ЗАПОЛНИТЬ ЗП
              </Button>
              <IconButton
                  className={styles.iconButton}
                  onClick={() => onEdit(report.id)}
              >
                  <EditIcon />
              </IconButton>
              <IconButton disabled className={styles.iconButton}>
                  <DeleteIcon />
              </IconButton>
          </div>

      </div>
    </div>
  );
};

export default ReportCard;
