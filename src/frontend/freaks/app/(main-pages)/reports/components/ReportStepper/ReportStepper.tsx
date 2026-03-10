"use client";

import { useEffect, useState } from "react";
import { FormProvider, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button, IconButton, Tooltip } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import { CustomContainer, CustomStepper, CustomTypography } from "@/components";
import { useCreateSalary } from "@/domains/reports/hooks/SalaryLoot/useCreateSalary";
import { useUpdateSalary } from "@/domains/reports/hooks/SalaryLoot/useUpdateSalary";
import { useGetSalaryById } from "@/domains/reports/hooks/SalaryLoot/useGetSalaryById";
import { useOpenRegistration } from "@/domains/reports/hooks/Registration/useOpenRegistration";
import {
  mapFormToRequest,
  step1Defaults,
  type Step1FormValues,
  step1Schema,
} from "./steps/Step-1/step1Schema";
import Step1 from "./steps/Step-1/Step1";
import Step2 from "./steps/Step-2/Step2";
import Step3 from "./steps/Step-3/Step3";
import Step4 from "./steps/Step4";
import Step5 from "./steps/Step5";
import Step6 from "./steps/Step6";
import styles from "./_styles.module.scss";

const STEPS = [
  "Параметры периода",
  "Продано за период",
  "Доля руководства",
  "Расходы и отчисления",
  "Итоговый отчет",
  "Распределение зарплат",
];

export type StepProps = {
  salaryId?: number;
};

const STEP_COMPONENTS = [Step1, Step2, Step3, Step4, Step5, Step6];

type Props = {
  onClose: () => void;
  salaryId?: number;
};

const ReportStepper = ({ onClose, salaryId: initialSalaryId }: Props) => {
  const [activeStep, setActiveStep] = useState(0);
  const [salaryId, setSalaryId] = useState<number | undefined>(initialSalaryId);
  const isEditing = !!initialSalaryId;

  const methods = useForm<Step1FormValues>({
    resolver: zodResolver(step1Schema),
    defaultValues: step1Defaults,
    mode: "onTouched",
  });

  const { salary } = useGetSalaryById(initialSalaryId);
  const { trigger: createSalary, isMutating: isCreating } = useCreateSalary();
  const { trigger: updateSalary, isMutating: isUpdating } = useUpdateSalary();
  const { trigger: openRegistration } = useOpenRegistration();

  const isMutating = isCreating || isUpdating;

  useEffect(() => {
    if (!salary) return;
    methods.reset({
      name: salary.name,
      startDt: new Date(salary.startDt).toISOString(),
      endDt: new Date(salary.endDt).toISOString(),
      allowedPaymentTypes: salary.allowedPaymentTypes,
      useCoefficients: salary.useCoefficients,
      bossTypes: salary.bossTypes,
    });
    void methods.trigger();
  }, [salary]);

  const isStep1Valid = methods.formState.isValid;

  const handleNext = async () => {
    if (activeStep === 0) {
      const valid = await methods.trigger();
      if (!valid) return;

      const formData = methods.getValues();
      const request = mapFormToRequest(formData);

      if (isEditing && salaryId) {
        const result = await updateSalary({ id: salaryId, data: request });
        if (!result) return;
      } else if (!salaryId) {
        const result = await createSalary(request);
        if (!result) return;
        await openRegistration(result.id);
        setSalaryId(result.id);
      }
    }

    if (activeStep < STEPS.length - 1) {
      setActiveStep((prev) => prev + 1);
    } else {
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

  const isNextDisabled = (activeStep === 0 && !isStep1Valid) || isMutating;

  const nextTooltip =
    activeStep === 0 && !isStep1Valid ? "Заполните обязательные поля" : "";

  return (
    <FormProvider {...methods}>
      <CustomContainer maxWidth="lg">
        <div className={styles.wrapper}>
          <div className={styles.header}>
            <div className={styles.headerLeft}>
              <CustomTypography variant="h4" fontWeight={700}>
                {isEditing ? "Редактирование периода" : "Новый отчетный период"}
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

          <CustomStepper steps={STEPS} activeStep={activeStep} />

          <div className={styles.contentContainer}>
            <CurrentStepComponent salaryId={salaryId} />
          </div>

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

            <Tooltip title={nextTooltip} placement="top" arrow>
              <span>
                <Button
                  variant="contained"
                  endIcon={!isLastStep && <ArrowForwardIcon />}
                  onClick={handleNext}
                  disabled={isNextDisabled}
                  className={styles.nextButton}
                >
                  {isMutating
                    ? "СОХРАНЕНИЕ..."
                    : isLastStep
                      ? "ЗАВЕРШИТЬ ПЕРИОД"
                      : STEPS[activeStep + 1]?.toUpperCase()}
                </Button>
              </span>
            </Tooltip>
          </div>
        </div>
      </CustomContainer>
    </FormProvider>
  );
};

export default ReportStepper;
