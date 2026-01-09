import { CustomTypography } from "@/components";

const Step2 = () => {
  return (
    <div style={{ padding: "40px", textAlign: "center" }}>
      <CustomTypography variant="h5" fontWeight={600}>
        Шаг 2: Продано за период
      </CustomTypography>
      <CustomTypography variant="body1" sx={{ marginTop: "16px", opacity: 0.7 }}>
        Здесь будет таблица проданных предметов
      </CustomTypography>
    </div>
  );
};

export default Step2;
