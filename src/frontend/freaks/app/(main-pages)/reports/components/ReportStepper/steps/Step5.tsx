import { CustomTypography } from "@/components";

const Step5 = () => {
  return (
    <div style={{ padding: "40px", textAlign: "center" }}>
      <CustomTypography variant="h5" fontWeight={600}>
        Шаг 5: Итоговый отчет
      </CustomTypography>
      <CustomTypography variant="body1" sx={{ marginTop: "16px", opacity: 0.7 }}>
        Здесь будет итоговая сводка по отчёту
      </CustomTypography>
    </div>
  );
};

export default Step5;
