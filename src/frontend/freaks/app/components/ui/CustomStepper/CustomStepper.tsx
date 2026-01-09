import { Stepper, Step, StepLabel, StepperProps } from "@mui/material";
import clsx from "clsx";
import styles from "./_styles.module.scss";

export interface CustomStepperProps extends Omit<StepperProps, "children"> {
  steps: string[];
  activeStep: number;
  className?: string;
}

const CustomStepper = ({
  steps,
  activeStep,
  className,
  ...props
}: CustomStepperProps) => {
  return (
    <div className={clsx(styles.stepperWrapper, className)}>
      <Stepper activeStep={activeStep} {...props}>
        {steps.map((label, index) => {
          const isActive = index === activeStep;
          const isCompleted = index < activeStep;

          return (
            <Step key={label} className={styles.step}>
              <StepLabel
                className={clsx(styles.stepLabel, {
                  [styles.active]: isActive,
                  [styles.completed]: isCompleted,
                })}
              >
                <div className={styles.stepContent}>
                  {/* Прогресс-бар */}
                  <div className={styles.progressBar}>
                    <div
                      className={clsx(styles.progressFill, {
                        [styles.progressActive]: isActive || isCompleted,
                      })}
                    />
                  </div>

                  {/* Номер и название */}
                  <div className={styles.stepInfo}>
                    <div
                      className={clsx(styles.stepNumber, {
                        [styles.numberActive]: isActive,
                      })}
                    >
                      {index + 1}
                    </div>
                    <div className={styles.stepName}>{label}</div>
                  </div>
                </div>
              </StepLabel>
            </Step>
          );
        })}
      </Stepper>
    </div>
  );
};

export default CustomStepper;
