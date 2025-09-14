import styles from "./_styles.module.scss";
import { CustomTypography } from "../CustomTypography";

type Props = {
  message?: string;
  variant?: React.ComponentProps<typeof CustomTypography>["variant"];
}; 

const NotFound = ({ message, variant = "h3" }: Props) => {
  return (
    <section className={styles.notFoundSection}>
      <CustomTypography variant={variant}>
        {message ?? "Данные не найдены"}
      </CustomTypography>
    </section>
  );
};

export default NotFound;
