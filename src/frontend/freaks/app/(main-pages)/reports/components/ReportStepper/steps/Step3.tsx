import { CustomTypography } from "@/components";

const Step3 = () => {
  return (
    <div style={{ padding: "40px", textAlign: "center" }}>
      <CustomTypography variant="h5" fontWeight={600}>
        Шаг 3: Долги ГЛ
      </CustomTypography>
      <CustomTypography variant="body1" sx={{ marginTop: "16px", opacity: 0.7 }}>
        Здесь будет таблица долгов гильд-лидера
      </CustomTypography>
    </div>
  );
};

export default Step3;
