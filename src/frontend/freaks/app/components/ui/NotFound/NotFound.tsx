import styles from "./_styles.module.scss";
import { CustomTypography } from "../CustomTypography";

type Props = {
  message?: string;
  variant?: React.ComponentProps<typeof CustomTypography>["variant"];
  align?: React.CSSProperties["justifyContent"];
};

const NotFound = ({ message, variant = "h3", align = "center" }: Props) => {
  return (
    <section
      style={{ justifyContent: align }}
      className={styles.notFoundSection}
    >
      <CustomTypography variant={variant}>
        {message ?? "Данные не найдены"}
      </CustomTypography>
    </section>
  );
};

export default NotFound;
