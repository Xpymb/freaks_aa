import { CustomTypography } from "@/components";

const Step4 = () => {
  return (
    <div style={{ padding: "40px", textAlign: "center" }}>
      <CustomTypography variant="h5" fontWeight={600}>
        Шаг 4: Расходы и отчисления
      </CustomTypography>
      <CustomTypography variant="body1" sx={{ marginTop: "16px", opacity: 0.7 }}>
        Здесь будет таблица расходов и отчислений
      </CustomTypography>
    </div>
  );
};

export default Step4;
