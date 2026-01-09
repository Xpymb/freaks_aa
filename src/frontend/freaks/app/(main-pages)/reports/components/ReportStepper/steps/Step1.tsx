import { CustomTypography } from "@/components";

const Step1 = () => {
  return (
    <div style={{ padding: "40px", textAlign: "center" }}>
      <CustomTypography variant="h5" fontWeight={600}>
        Шаг 1: Параметры периода
      </CustomTypography>
      <CustomTypography variant="body1" sx={{ marginTop: "16px", opacity: 0.7 }}>
        Здесь будет форма настройки параметров отчётного периода
      </CustomTypography>
    </div>
  );
};

export default Step1;
