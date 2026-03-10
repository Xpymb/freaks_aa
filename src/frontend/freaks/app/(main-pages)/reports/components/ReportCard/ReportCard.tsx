"use client";

import { SalaryDto } from "@/domains/reports";
import { useDeleteSalary } from "@/domains/reports/hooks/SalaryLoot/useDeleteSalary";
import { useGetSalaries } from "@/domains/reports/hooks/SalaryLoot/useGetSalaries";
import { CustomTypography } from "@/components";
import { Button, IconButton } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import ReportStatusBadge from "../ReportStatusBadge/ReportStatusBadge";
import styles from "./_styles.module.scss";

type Props = {
  salary: SalaryDto;
  onEdit: (salaryId: number) => void;
};

const ReportCard = ({ salary, onEdit }: Props) => {
  const { trigger: deleteSalary, isMutating } = useDeleteSalary();
  const { refresh } = useGetSalaries();

  const handleDelete = async () => {
    await deleteSalary(salary.id);
    refresh();
  };

  return (
    <div className={styles.salary}>
      <div className={styles.left}>
        <div className={styles.period}>
          <CustomTypography variant="h4" fontWeight={600}>
            {salary.name}
          </CustomTypography>
        </div>

        {/*<div className={styles.steps}>*/}
        {/*  {Object.entries(Salary.fillStepType).map(([key, completed]) => (*/}
        {/*    <div key={key} className={styles.parameter}>*/}
        {/*      {completed ? (*/}
        {/*        <Image*/}
        {/*          src={"/icons/complete_step_icon.svg"}*/}
        {/*          alt="overviewIcon"*/}
        {/*          width={16}*/}
        {/*          height={16}*/}
        {/*        />*/}
        {/*      ) : (*/}
        {/*        <Image*/}
        {/*          src={"/icons/uncomplete_step_icon.svg"}*/}
        {/*          alt="overviewIcon"*/}
        {/*          width={16}*/}
        {/*          height={16}*/}
        {/*        />*/}
        {/*      )}*/}
        {/*      <CustomTypography variant="caption">*/}
        {/*        {*/}
        {/*          SALARY_PARAMETER_LABELS[*/}
        {/*            key as keyof typeof SALARY_PARAMETER_LABELS*/}
        {/*          ]*/}
        {/*        }*/}
        {/*      </CustomTypography>*/}
        {/*    </div>*/}
        {/*  ))}*/}
        {/*</div>*/}
      </div>

      {/* Правая часть - Статус и кнопки */}
      <div className={styles.right}>
        <ReportStatusBadge status={salary.registrationStatus} />
        <div className={styles.controlBtn}>
          <Button variant="outlined" disabled className={styles.fillButton}>
            ЗАПОЛНИТЬ ЗП
          </Button>
          <IconButton
            className={styles.iconButton}
            onClick={() => onEdit(salary.id)}
          >
            <EditIcon />
          </IconButton>
          <IconButton
            className={styles.iconButton}
            onClick={handleDelete}
            disabled={isMutating}
          >
            <DeleteIcon />
          </IconButton>
        </div>
      </div>
    </div>
  );
};

export default ReportCard;
