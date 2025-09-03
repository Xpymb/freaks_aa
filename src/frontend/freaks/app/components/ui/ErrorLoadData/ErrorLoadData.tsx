import { CustomTypography } from "../CustomTypography";
import styles from "./_styles.module.scss";

const ErrorLoadData = () => {
  return (
    <section className={styles.error}>
      <CustomTypography variant="h3" color="error">
        Произошла ошибка при загрузке данных...
      </CustomTypography>
    </section>
  );
};

export default ErrorLoadData;
