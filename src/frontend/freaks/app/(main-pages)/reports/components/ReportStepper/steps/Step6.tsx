import { CustomTypography } from "@/components";
import type { StepProps } from "../ReportStepper";

const Step6 = ({ salaryId }: StepProps) => {
  return (
    <div style={{ padding: "40px", textAlign: "center" }}>
      <CustomTypography variant="h5" fontWeight={600}>
        Шаг 6: Распределение зарплат
      </CustomTypography>
      <CustomTypography variant="body1" sx={{ marginTop: "16px", opacity: 0.7 }}>
        Здесь будет таблица распределения зарплат участников
      </CustomTypography>
    </div>
  );
};

export default Step6;
