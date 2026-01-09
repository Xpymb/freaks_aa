"use client";

import {useState} from "react";
import {Button, IconButton} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import {CustomContainer, CustomStepper, CustomTypography} from "@/components";
import {MOCK_REPORTS} from "@/domains/reports";
import Step1 from "./steps/Step1";
import Step2 from "./steps/Step2";
import Step3 from "./steps/Step3";
import Step4 from "./steps/Step4";
import Step5 from "./steps/Step5";
import Step6 from "./steps/Step6";
import styles from "./_styles.module.scss";

const STEPS = [
  "Параметры периода",
  "Продано за период",
  "Долги ГЛ",
  "Расходы и отчисления",
  "Итоговый отчет",
  "Распределение зарплат",
];

const STEP_COMPONENTS = [Step1, Step2, Step3, Step4, Step5, Step6];

type Props = {
  onClose: () => void;
  reportId?: number;
};

const ReportStepper = ({ onClose, reportId }: Props) => {
  const [activeStep, setActiveStep] = useState(0);

  // Находим отчет по ID, если он передан
  const report = reportId
    ? MOCK_REPORTS.find((r) => r.id === reportId)
    : undefined;

  const handleNext = () => {
    if (activeStep < STEPS.length - 1) {
      setActiveStep((prev) => prev + 1);
    } else {
      // Последний шаг - завершить
      onClose();
    }
  };

  const handleBack = () => {
    if (activeStep > 0) {
      setActiveStep((prev) => prev - 1);
    }
  };

  const CurrentStepComponent = STEP_COMPONENTS[activeStep];
  const isFirstStep = activeStep === 0;
  const isLastStep = activeStep === STEPS.length - 1;

  return (
    <CustomContainer maxWidth="lg">
      <div className={styles.wrapper}>
        {/* Заголовок */}
        <div className={styles.header}>
          <div className={styles.headerLeft}>
            <CustomTypography variant="h4" fontWeight={700}>
              {report
                ? `Редактирование отчета: ${report.period}`
                : "Новый отчетный период"}
            </CustomTypography>
          </div>
          <IconButton
            onClick={onClose}
            className={styles.closeButton}
            size="small"
          >
            <CloseIcon />
          </IconButton>
        </div>

        {/* Степпер */}
        <CustomStepper steps={STEPS} activeStep={activeStep} />

        {/* Контент текущего шага */}
        <div className={styles.contentContainer}>
          <CurrentStepComponent />
        </div>

        {/* Footer с кнопками навигации */}
        <div className={styles.footer}>
          <Button
            variant="outlined"
            startIcon={<ArrowBackIcon />}
            onClick={handleBack}
            disabled={isFirstStep}
            className={styles.backButton}
          >
            НАЗАД
          </Button>

          <Button
            variant="contained"
            endIcon={!isLastStep && <ArrowForwardIcon />}
            onClick={handleNext}
            className={styles.nextButton}
          >
            {isLastStep ? "ЗАВЕРШИТЬ ПЕРИОД" : STEPS[activeStep + 1]?.toUpperCase()}
          </Button>
        </div>
      </div>
    </CustomContainer>
  );
};

export default ReportStepper;
