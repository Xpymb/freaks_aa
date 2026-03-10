import React from "react";
import { CustomTypography } from "@/components";
import styles from "./_styles.module.scss";

type Props = {
  title: string;
  subtitle: string;
};

const TitleBlock = ({ title, subtitle }: Props) => {
  return (
    <div className={styles.wrapper}>
      <CustomTypography variant="h4" className={styles.title}>
        {title}
      </CustomTypography>
      <CustomTypography variant="h6" className={styles.subtitle}>
        {subtitle}
      </CustomTypography>
    </div>
  );
};
export default TitleBlock;
