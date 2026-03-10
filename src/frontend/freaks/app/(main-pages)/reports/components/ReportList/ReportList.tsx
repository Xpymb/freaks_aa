"use client";

import { Button } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import { CustomContainer, CustomTypography } from "@/components";
import ReportCard from "../ReportCard/ReportCard";
import ReportFilters from "../ReportFilters/ReportFilters";
import styles from "./_styles.module.scss";
import { useGetSalaries } from "@/domains/reports/hooks/SalaryLoot/useGetSalaries";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";

type Props = {
  onCreateNew: () => void;
  onEdit: (salaryId: number) => void;
};

const ReportList = ({ onCreateNew, onEdit }: Props) => {
  const { salaries, isLoading, errorState } = useGetSalaries();

  if (isLoading || !salaries) return <DefaultLoader />;

  if (errorState.isError) {
    return <ErrorLoadData message={errorState.message} />;
  }

  const containerVariants = {
    hidden: { opacity: 0 },
    visible: {
      opacity: 1,
      transition: {
        delayChildren: 0.2,
        staggerChildren: 0.1,
      },
    },
  };

  return (
    <CustomContainer maxWidth="lg">
      <div className={styles.wrapper}>
        <div className={styles.header}>
          <CustomTypography variant="h4" fontWeight={700}>
            Отчеты
          </CustomTypography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            className={styles.newReportButton}
            onClick={onCreateNew}
          >
            НОВЫЙ ОТЧЕТ
          </Button>
        </div>

        <ReportFilters />

        <div className={styles.reportsList}>
          {salaries.map((salary) => (
            <ReportCard key={salary.id} salary={salary} onEdit={onEdit} />
          ))}
        </div>
      </div>
    </CustomContainer>
  );
};

export default ReportList;
