import { CustomTypography } from "../CustomTypography";
import styles from "./_styles.module.scss";

type Props = {
  message?: string | null;
};

const ErrorLoadData = ({
  message = "Произошла ошибка при загрузке данных...",
}: Props) => {
  return (
    <section className={styles.error}>
      <CustomTypography variant="h3">{message}</CustomTypography>
    </section>
  );
};

export default ErrorLoadData;
