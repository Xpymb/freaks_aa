"use client";

import { motion } from "framer-motion";
import { Button } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import { CustomContainer, CustomTypography } from "@/components";
import ReportCard from "../ReportCard/ReportCard";
import ReportFilters from "../ReportFilters/ReportFilters";
import { MOCK_REPORTS } from "@/domains/reports";
import styles from "./_styles.module.scss";

type Props = {
  onCreateNew: () => void;
  onEdit: (reportId: number) => void;
};

const ReportList = ({ onCreateNew, onEdit }: Props) => {
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

  const itemVariants = {
    hidden: { y: 20, opacity: 0 },
    visible: {
      y: 0,
      opacity: 1,
      transition: {
        duration: 0.5,
        ease: "easeOut" as const,
      },
    },
  };

  return (
    <CustomContainer maxWidth="lg">
      <div className={styles.wrapper}>
        {/* Заголовок и кнопка */}
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

        {/* Фильтры */}
        <ReportFilters />

        {/* Список отчетов */}
        <motion.div
          className={styles.reportsList}
          variants={containerVariants}
          initial="hidden"
          animate="visible"
        >
          {MOCK_REPORTS.map((report) => (
            <motion.div
              key={report.id}
              variants={itemVariants}
              whileHover={{ scale: 1.02 }}
              transition={{ duration: 0.2 }}
            >
              <ReportCard report={report} onEdit={onEdit} />
            </motion.div>
          ))}
        </motion.div>
      </div>
    </CustomContainer>
  );
};

export default ReportList;
